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
using ITGame.Models.Entities;

namespace ITGame.DBManager.ViewModels
{
    public abstract class EntitiesViewModel<TEntity> : BaseViewModel, IEntitiesViewModel where TEntity : class, Identity, IViewModelItem, new()
    {
        #region Fields
        private readonly IEntityRepository _repository;
        private readonly IEntityProjector<TEntity> _entitiesContext;

        private ObservableCollection<TEntity> _entities;
        private TEntity _selectedEntity;

        private ICommand _commandLoadEntities;
        private ICommand _commandSaveEntities;
        private ICommand _commandDeleteSelectedEntities;
        private ICommand _commandCreateEntity;

        #endregion

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
            _commandDeleteSelectedEntities = new RelayCommand(DeleteSelectedEntities);
            _commandCreateEntity = new RelayCommand(CreateEntity);
        }

        private void CreateEntity(object obj)
        {
            var item = new TEntity() {Id = Guid.NewGuid()};
            Entities.Add(item);
            SelectedEntity = item;
            EntitiesContext.Add(item);
        }

        private void DeleteSelectedEntities(object obj)
        {
            if (SelectedEntity != null)
            {
                EntitiesContext.Delete(SelectedEntity);
                Entities.Remove(SelectedEntity);
            }
            var toDelete = Entities.Where(x => x.IsSelectedModelItem).ToList();
            foreach (var entity in toDelete)
            {
                EntitiesContext.Delete(entity);
                Entities.Remove(entity);
            }
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
            get { return _commandDeleteSelectedEntities; }
        }

        public virtual ICommand CommandDeleteAllEntities
        {
            get { return new RelayCommand(o => { }); }
        }

        public virtual ICommand CommandCreateEntity
        {
            get { return _commandCreateEntity; }
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
