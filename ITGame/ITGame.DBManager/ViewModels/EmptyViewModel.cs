using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ITGame.DBManager.Commands;
using ITGame.Infrastructure.Data;

namespace ITGame.DBManager.ViewModels
{
    public class EmptyViewModel : IEntitiesViewModel
    {
        public EmptyViewModel(IEntityRepository repository)
        {
            
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
    }
}
