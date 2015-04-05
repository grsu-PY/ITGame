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
    public abstract class EntitiesViewModel<TEntity> : BaseViewModel, IEntitiesViewModel where TEntity : class, Identity, new()
    public abstract class EntitiesViewModel<TEntity> : BaseViewModel, IEntitiesViewModel where TEntity : class, Identity, IViewModelItem, new()
    {
        private readonly IEntityRepository _repository;
        protected ObservableCollection<TEntity> _entities;
        private IEntityProjector<TEntity> _entitiesContext;

        protected ICommand _commandLoadEntities;
        protected ICommand _commandSaveEntities;
        private TEntity _selectedEntity;

        public EntitiesViewModel(IEntityRepository repository)
        {
            _repository = repository;
            _entities = new ObservableCollection<TEntity>();
            _entitiesContext = repository.GetInstance<TEntity>();

            InitializeCommands();
        }

        #region Commands
        private void InitializeCommands()
        {
            _commandLoadEntities = new RelayCommand(LoadEntities);
            _commandSaveEntities = new RelayCommand(SaveEntities);
        }

        private void SaveEntities(object obj)
        {
            foreach (var entity in Entities)
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }
                _entitiesContext.AddOrUpdate(entity);
            }
            _entitiesContext.SaveChanges();
        }

        private void LoadEntities(object o)
        {
            Entities = new ObservableCollection<TEntity>(EntitiesContext.GetAll());
        }
        public virtual ICommand CommandLoadEntities
        {
            get { return _commandLoadEntities; }
        }

        public virtual ICommand CommandSaveEntities
        {
            get { return _commandSaveEntities; }
        }

        public virtual ICommand CommandEditEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        public virtual ICommand CommandUpdateEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        public virtual ICommand CommandDeleteEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        public virtual ICommand CommandDeleteSelectedEntities
        {
            get { return new RelayCommand(o => { });}
        }

        public virtual ICommand CommandDeleteAllEntities
        {
            get { return new RelayCommand(o => { }); }
        }

        public virtual ICommand CommandCreateEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        #endregion

        #region Properties
        public virtual TEntity SelectedEntity
        {
            get { return _selectedEntity; }
            set
            {
                _selectedEntity = value;
                RaisePropertyChanged();
            }
        }

        public virtual ObservableCollection<TEntity> Entities
        {
            get { return _entities; }
            set
            {
                _entities = value;
                RaisePropertyChanged();
            }
        }
        protected IEntityProjector<TEntity> EntitiesContext
        {
            get { return _entitiesContext; }
        }

        protected IEntityRepository Repository
        {
            get { return _repository; }
        }
        #endregion
    }
}
