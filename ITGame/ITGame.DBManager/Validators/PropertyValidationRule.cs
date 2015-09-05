using System.Linq;
using FluentValidation.Internal;

namespace ITGame.DBManager.Validators
{
    public class PropertyValidationRule
    {
        public bool IsAsync { get; }

        public string Property { get; }

        public PropertyRule Rule { get; }

        public PropertyValidationRule(PropertyRule rule)
        {
            Rule = rule;
            IsAsync = Rule.Validators.Any(validator => validator.IsAsync);
            Property = Rule.PropertyName;
        }
    }
}