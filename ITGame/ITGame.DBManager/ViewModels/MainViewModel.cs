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
        private readonly static IList<NameValueItem<Type>> _entityViewModelTypes = new List<NameValueItem<Type>>()
        {
            new NameValueItem<Type> {Name = "Humanoids", Value = typeof (HumanoidsViewModel)},
            new NameValueItem<Type> {Name = "Armors", Value = typeof (ArmorsViewModel)},
        };

        private Type _selectedEntityType;
        private readonly IEntityRepository _repository;
        private readonly IEntityViewModelBuilder _entityViewModelBuilder;

        public MainViewModel(IEntityRepository repository, IEntityViewModelBuilder entityViewModelBuilder)
        {
            _repository = repository;
            _entityViewModelBuilder = entityViewModelBuilder;
        }

        public IList<NameValueItem<Type>> EntityViewModelTypes
        {
            get { return _entityViewModelTypes; }
        }

        public Type SelectedEntityType
        {
            get { return _selectedEntityType; }
            set
            {
                _selectedEntityType = value;

                EntityViewModel = _entityViewModelBuilder.Resolve(_selectedEntityType, _repository) as IEntitiesViewModel;
                RaisePropertyChanged();
            }
        }

        private IEntitiesViewModel _entityViewModel;

        public IEntitiesViewModel EntityViewModel
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
