using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ITGame.DBManager.Extensions;
using ITGame.DBManager.Validators;
using ValidationContext = FluentValidation.ValidationContext;

namespace ITGame.DBManager.ViewModels
{
    public class ValidationViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly object _lockObject = new object();
        private readonly ConcurrentDictionary<string, List<string>> _errors = new ConcurrentDictionary<string, List<string>>();
        private Dictionary<string, List<IValidationRule>> _validators;
        private Dictionary<string, List<IValidationRule>> _asyncValidators;
        private IValidator _validator;
        private bool _startUpValidated = false;

        protected IValidator Validator
        {
            get { return _validator; }
            set
            {
                _validator = value;
                ExtractValidators(value, out _validators, out _asyncValidators);
            }
        }

        private void ExtractValidators(
            IValidator validator,
            out Dictionary<string, List<IValidationRule>> validators,
            out Dictionary<string, List<IValidationRule>> asyncValidators)
        {
            validators = new Dictionary<string, List<IValidationRule>>();
            asyncValidators = new Dictionary<string, List<IValidationRule>>();

            if (validator != null)
            {
                var propertyRules = validator.AllPropertyRules()
                    .Select(rule => new PropertyValidationRule(rule))
                    .GroupBy(rule => rule.Property);

                foreach (var propertyRule in propertyRules)
                {
                    var propertyName = propertyRule.Key;

                    foreach (var propertyValidationRule in propertyRule)
                    {
                        if (propertyValidationRule.IsAsync)
                        {
                            if (!asyncValidators.ContainsKey(propertyName))
                            {
                                asyncValidators.Add(propertyName, new List<IValidationRule>());
                            }
                            asyncValidators[propertyName].Add(propertyValidationRule.Rule);
                        }
                        else
                        {
                            if (!validators.ContainsKey(propertyName))
                            {
                                validators.Add(propertyName, new List<IValidationRule>());
                            }
                            validators[propertyName].Add(propertyValidationRule.Rule);
                        }
                    }
                }
            }
        }

        protected override void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);

            if (string.IsNullOrEmpty(propertyName))
            {
                ValidateAsync();
            }
            else
            {
                Validate(propertyName);
                ValidateAsync(propertyName);
            }
        }

        protected override void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            base.RaisePropertyChanged(property);
            var propertyName = property.ExpressionMemberName();

            if (string.IsNullOrEmpty(propertyName))
            {
                ValidateAsync();
            }
            else
            {
                Validate(propertyName);
                ValidateAsync(propertyName);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            return _errors.ValueOrDefault(propertyName);
        }

        public void Validate(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && _validators != null && _validators.ContainsKey(propertyName))
            {
                var validatorRules = _validators[propertyName];
                bool hasErros = false;
                List<ValidationFailure> errors = new List<ValidationFailure>();
                foreach (var validationRule in validatorRules)
                {
                    errors.AddRange(validationRule.Validate(new ValidationContext(this)));
                }
                hasErros = errors.Any();

                if (!hasErros)
                {
                    List<string> list;
                    _errors.TryRemove(propertyName, out list);
                    RaiseErrorsChanged(propertyName);
                }
                else
                {
                    _errors.AddOrUpdate(propertyName,
                        propName => errors.Select(failure => failure.ErrorMessage).ToList(),
                        (propName, list) =>
                        {
                            list.Clear();
                            list.AddRange(errors.Select(failure => failure.ErrorMessage));
                            return list;
                        });

                    RaiseErrorsChanged(propertyName);
                }
            }
        }

        public async Task ValidateAsync(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && _asyncValidators != null &&
                _asyncValidators.ContainsKey(propertyName))
            {
                var validatorRules = _asyncValidators[propertyName];
                bool hasErros = false;
                var validationTasks = new List<Task<IEnumerable<ValidationFailure>>>(
                    validatorRules.Select(rule => rule.ValidateAsync(new ValidationContext(this))));

                var validationResults = await Task.WhenAll(validationTasks);
                var errors = validationResults.SelectMany(failures => failures.ToList()).ToList();
                hasErros = errors.Any();

                if (!hasErros)
                {
                    List<string> list;
                    _errors.TryRemove(propertyName, out list);
                    RaiseErrorsChanged(propertyName);
                }
                else
                {
                    _errors.AddOrUpdate(propertyName,
                        propName => errors.Select(failure => failure.ErrorMessage).ToList(),
                        (propName, list) =>
                        {
                            list.Clear();
                            list.AddRange(errors.Select(failure => failure.ErrorMessage));
                            return list;
                        });

                    RaiseErrorsChanged(propertyName);
                }
            }
        }

        public Task ValidateAsync()
        {
            return Task.Run(() => Validate());
        }

        public void Validate()
        {
            lock (_lockObject)
            {
                if (Validator == null) return;
                var validationResult = Validator.Validate(this);
                if (validationResult.IsValid)
                {
                    _errors.Clear();
                    RaiseErrorsChanged(string.Empty);
                }
                else
                {
                    var propErrors =
                        from failure in validationResult.Errors
                        group failure by failure.PropertyName
                        into groupedFailures
                        select groupedFailures;

                    var toDelete = _errors.Keys.Where(key => propErrors.All(prop => prop.Key != key));

                    foreach (var keyToDelete in toDelete)
                    {
                        List<string> list;
                        _errors.TryRemove(keyToDelete, out list);
                        RaiseErrorsChanged(keyToDelete);
                    }

                    foreach (var validationFailure in propErrors)
                    {
                        var propertyName = validationFailure.Key;

                        _errors.AddOrUpdate(propertyName,
                            propName => validationFailure.Select(failure => failure.ErrorMessage).ToList(),
                            (propName, list) =>
                            {
                                list.Clear();
                                list.AddRange(validationFailure.Select(failure => failure.ErrorMessage));
                                return list;
                            });

                        RaiseErrorsChanged(propertyName);
                    }
                }
            }
        }

        public bool HasErrors
        {
            get { return _errors.Any(kv => kv.Value != null && kv.Value.Count > 0); }
        }

        protected virtual void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            handler?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
