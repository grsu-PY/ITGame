using System.Windows.Input;
using ITGame.DBManager.Commands;
using ITGame.DBManager.Navigations;
using ITGame.Infrastructure.Data;

namespace ITGame.DBManager.ViewModels
{
    public class EmptyViewModel : IEntitiesViewModel, INavigatableViewModel
    {
        public EmptyViewModel(INavigation navigation, IEntityRepository repository)
        {
            
        }

        public object SelectedEntity
        {
            get { return null; }
        }

        public ICommand CommandLoadEntities
        {
            get { return new RelayCommand(o => { }); }
        }

        public ICommand CommandSaveEntities
        {
            get { return new RelayCommand(o => { }); }
        }

        public ICommand CommandEditEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        public ICommand CommandUpdateEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        public ICommand CommandDeleteEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        public ICommand CommandDeleteSelectedEntities
        {
            get { return new RelayCommand(o => { }); }
        }

        public ICommand CommandDeleteAllEntities
        {
            get { return new RelayCommand(o => { }); }
        }

        public ICommand CommandCreateEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        public void OnNavigated()
        {
            
        }

        public void OnBeforeNavigation()
        {
            
        }
    }
}
