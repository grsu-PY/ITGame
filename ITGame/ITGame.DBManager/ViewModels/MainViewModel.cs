using ITGame.DBConnector;
using ITGame.DBManager.Data;
using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.DBManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private static IList<Type> _entityViewModelTypes = new List<Type>() 
        { 
            typeof(HumanoidsViewModel), 
            typeof(ArmorsViewModel) 
        };

        private Type _selectedEntityType;
        private IEntityRepository _repository;
        private IEntityViewModelBuilder _entityViewModelBuilder;

        public MainViewModel(IEntityRepository repository, IEntityViewModelBuilder entityViewModelBuilder)
        {
            _repository = repository;
            _entityViewModelBuilder = entityViewModelBuilder;
        }

        public IList<Type> EntityViewModelTypes
        {
            get { return _entityViewModelTypes; }
        }

        public Type SelectedEntityType
        {
            get { return _selectedEntityType; }
            set
            {
                _selectedEntityType = value;

                EntityViewModel = _entityViewModelBuilder.Resolve(_selectedEntityType, _repository);
                RaisePropertyChanged();
            }
        }

        private object _entityViewModel;

        public object EntityViewModel
        {
            get { return _entityViewModel; }
            set
            {
                _entityViewModel = value;
                RaisePropertyChanged();
            }
        }

    }
}
