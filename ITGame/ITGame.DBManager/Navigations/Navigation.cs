using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ITGame.DBManager.Annotations;
using ITGame.DBManager.Data;
using ITGame.DBManager.Extensions;
using Microsoft.Practices.Unity;

namespace ITGame.DBManager.Navigations
{
    public class Navigation : INavigation, INotifyPropertyChanged
    {
        private const string CachedViewName = "cached";
        private readonly IUnityContainer _unityContainer;
        public event EventHandler<ViewChangedEventArgs> ViewChanged; 
        private  INavigatableViewModel _currentEntityViewModel;
        private  Type _selectedEntityViewType;

        public Navigation(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public Type SelectedEntityViewType
        {
            get { return _selectedEntityViewType; }
            private set
            {
                _selectedEntityViewType = value;
                OnPropertyChanged();
            }
        }

        public INavigatableViewModel CurrentEntityViewModel
        {
            get { return _currentEntityViewModel; }
            private set
            {
                _currentEntityViewModel = value;
                OnPropertyChanged();
            }
        }

        public void SwitchToView(Type viewType)
        {
            OnBeforeNavigation();

            CurrentEntityViewModel = _unityContainer.Resolve(viewType, CachedViewName) as INavigatableViewModel;
            SelectedEntityViewType = viewType;

            OnNavigated();
        }

        public void SwitchToView(Type viewType, object parameters)
        {
            OnBeforeNavigation();

            CurrentEntityViewModel = _unityContainer.ResolveExt(viewType, CachedViewName, parameters) as INavigatableViewModel;
            SelectedEntityViewType = viewType;

            OnNavigated();
        }

        public void SwitchToViewNotCached(Type viewType)
        {
            OnBeforeNavigation();

            CurrentEntityViewModel = _unityContainer.Resolve(viewType) as INavigatableViewModel;
            _unityContainer.RegisterInstance(viewType, CachedViewName, CurrentEntityViewModel, new ContainerControlledLifetimeManager());
            SelectedEntityViewType = viewType;

            OnNavigated();
        }

        public void SwitchToViewNotCached(Type viewType, object parameters)
        {
            OnBeforeNavigation();

            CurrentEntityViewModel = _unityContainer.ResolveExt(viewType, parameters) as INavigatableViewModel;
            _unityContainer.RegisterInstance(viewType, CachedViewName, CurrentEntityViewModel, new ContainerControlledLifetimeManager());
            SelectedEntityViewType = viewType;

            OnNavigated();
        }

        private void OnNavigated()
        {
            if (CurrentEntityViewModel != null)
            {
                CurrentEntityViewModel.OnNavigated();
            }
        }

        private void OnBeforeNavigation()
        {
            if (CurrentEntityViewModel != null)
            {
                CurrentEntityViewModel.OnBeforeNavigation();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}