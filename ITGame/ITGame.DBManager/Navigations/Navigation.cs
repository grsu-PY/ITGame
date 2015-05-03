using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ITGame.DBManager.Annotations;
using ITGame.DBManager.Data;

namespace ITGame.DBManager.Navigations
{
    public class Navigation : INavigation, INotifyPropertyChanged
    {
        public event EventHandler<ViewChangedEventArgs> ViewChanged; 
        private  INavigatableViewModel _currentEntityViewModel;
        private  Type _selectedEntityViewType;
        private readonly IEntityViewModelBuilder _entityViewModelBuilder;

        public Navigation(IEntityViewModelBuilder entityViewModelBuilder)
        {
            _entityViewModelBuilder = entityViewModelBuilder;
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
        
        public void SwitchToView(Type viewType, params object[] parameters)
        {
            OnBeforeNavigation();

            CurrentEntityViewModel =
                _entityViewModelBuilder.Resolve(viewType, CreateParameters(parameters)) as INavigatableViewModel;
            SelectedEntityViewType = viewType;

            OnNavigated();
        }

        public void SwitchToViewNotCached(Type viewType, params object[] parameters)
        {
            OnBeforeNavigation();

            CurrentEntityViewModel =
                _entityViewModelBuilder.Resolve(viewType, false, CreateParameters(parameters)) as INavigatableViewModel;
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

        private object[] CreateParameters(object[] parameters)
        {
            object[] paramsObjects = new object[parameters.Length + 1];
            paramsObjects[0] = this;
            for (int i = 1; i < paramsObjects.Length; i++)
            {
                paramsObjects[i] = parameters[i - 1];
            }
            return paramsObjects;
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