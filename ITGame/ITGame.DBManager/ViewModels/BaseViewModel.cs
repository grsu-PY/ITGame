using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ITGame.DBManager.Navigations;

namespace ITGame.DBManager.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INavigatableViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly INavigation _navigation;

        public BaseViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        public INavigation Navigation
        {
            get { return _navigation; }
        }

        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null && !string.IsNullOrEmpty(propertyName))
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, property.CreateChangeEventArgs());
            }
        }

        public virtual void OnNavigated() { }
        public virtual void OnBeforeNavigation() { }
    }
    public static class PropertyExtensions
    {
        public static PropertyChangedEventArgs CreateChangeEventArgs<T>(this Expression<Func<T>> property)
        {
            var expression = property.Body as MemberExpression;
            MemberInfo member = expression.Member;

            return new PropertyChangedEventArgs(member.Name);
        }
    }
}
