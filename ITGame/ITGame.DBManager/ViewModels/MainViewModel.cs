using ITGame.DBConnector;
using ITGame.DBManager.Data;
using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ITGame.DBManager.Navigations;

namespace ITGame.DBManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly static IList<NameValueItem<Type>> _entityViewModelTypes = new List<NameValueItem<Type>>()
        {
            new NameValueItem<Type> {Name = "None", Value = typeof (EmptyViewModel)},
            new NameValueItem<Type> {Name = "Humanoids", Value = typeof (HumanoidsViewModel)},
            new NameValueItem<Type> {Name = "Armors", Value = typeof (ArmorsViewModel)},
            new NameValueItem<Type> {Name = "Characters", Value = typeof (CharactersViewModel)},
        };

        private Type _selectedEntityType;
        private readonly IEntityRepository _repository;

        public MainViewModel(INavigation navigation, IEntityRepository repository) : base(navigation)
        {
            _repository = repository;
            SelectedEntityType = _entityViewModelTypes[0].Value;
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

                Navigation.SwitchToView(_selectedEntityType, _repository);
                RaisePropertyChanged();
            }
        }
        
    }
}
