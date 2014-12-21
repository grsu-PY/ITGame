using ITGame.DBConnector;
using ITGame.DBManager.Commands;
using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ITGame.DBManager.ViewModels
{
    public abstract class EntitiesViewModel<TEntity> : BaseViewModel where TEntity : class, ITGame.Models.Identity, new()
    {
        protected ObservableCollection<TEntity> _entities;
        private IEntityProjector<TEntity> _dbContext;

        protected ICommand _commandLoadEntities;

        public EntitiesViewModel(IEntityRepository repository)
        {
            _entities = new ObservableCollection<TEntity>();
            _dbContext = repository.GetInstance<TEntity>();

            InitializeCommands();
        }

        #region Commands
        private void InitializeCommands()
        {
            _commandLoadEntities = new RelayCommand(o => LoadEntities(o));
        }

        private void LoadEntities(object o)
        {
            Entities = new ObservableCollection<TEntity>(EntityDBContext.GetAll());
        }
        public virtual ICommand CommandLoadEntities
        {
            get { return _commandLoadEntities; }
        }
        #endregion

        public virtual ObservableCollection<TEntity> Entities
        {
            get { return _entities; }
            set
            {
                _entities = value;
                RaisePropertyChanged();
            }
        }
        protected IEntityProjector<TEntity> EntityDBContext
        {
            get { return _dbContext; }
        }
    }
}
