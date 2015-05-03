using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ITGame.DBManager.Annotations;
using ITGame.DBManager.Data;
using ITGame.DBManager.ViewModels;

namespace ITGame.DBManager.Navigations
{
    public class Navigation : INavigation, INotifyPropertyChanged
    {
        public event EventHandler<ViewChangedEventArgs> ViewChanged; 
        private  object _currentEntityViewModel;
        private  Type _selectedEntityViewType;
        private readonly IEntityViewModelBuilder _entityViewModelBuilder;

        public Navigation(IEntityViewModelBuilder entityViewModelBuilder)
        {
            _entityViewModelBuilder = entityViewModelBuilder;
        }

        public Type SelectedEntityViewType
        {
            get { return _selectedEntityViewType; }
            set
            {
                _selectedEntityViewType = value;
                OnPropertyChanged();
            }
        }

        public object CurrentEntityViewModel
        {
            get { return _currentEntityViewModel; }
            set
            {
                _currentEntityViewModel = value;
                OnPropertyChanged();
            }
        }
        
        public void SwitchToView(Type viewType, params object[] parameters)
        {

            CurrentEntityViewModel = _entityViewModelBuilder.Resolve(viewType, CreateParameters(parameters));
            SelectedEntityViewType = viewType;
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