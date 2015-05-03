using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ITGame.DBManager.Commands;
using ITGame.DBManager.Converters;
using ITGame.DBManager.Data;
using ITGame.DBManager.Navigations;
using ITGame.Models.Сreature;

namespace ITGame.DBManager.ViewModels
{
    public class HumanoidsViewModel : EntitiesViewModel<Models.Entities.Humanoid>
    {
        private ICommand _commandEditEntity;

        public HumanoidsViewModel(INavigation navigation, IEntityRepository repository)
            : base(navigation, repository)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _commandEditEntity = new RelayCommand(EditEntity);
        }

        private void EditEntity(object obj)
        {
            Navigation.SwitchToView(typeof(HumanoidViewModel), obj ?? new Models.Entities.Humanoid());
        }

        public IEnumerable<NameValueItem<object>> HumanoidRacesList
        {
            get { return NameValueEnumCollections.GetCollection(typeof(HumanoidRaceType)); }
        }

        public override ICommand CommandEditEntity
        {
            get { return _commandEditEntity; }
        }
    }
}
