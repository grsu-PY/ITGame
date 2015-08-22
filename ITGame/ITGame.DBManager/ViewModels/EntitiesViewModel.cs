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
using ITGame.DBManager.Data;
using ITGame.DBManager.Navigations;
using ITGame.Models.Entities;

namespace ITGame.DBManager.ViewModels
{
    public abstract class EntitiesViewModel<TEntity> : BaseViewModel, IEntitiesViewModel, IPagedViewModel where TEntity : class, Identity, IViewModelItem, new()
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
        private ICommand _commandEditEntity;
        private ICommand _commandNextPage;
        private ICommand _commandPreviousPage;

        #endregion

        public EntitiesViewModel(INavigation navigation, IEntityRepository repository) : base(navigation)
        {
            _repository = repository;
            _entities = new ObservableCollection<TEntity>();
            _entitiesContext = repository.GetInstance<TEntity>();

            PageInfo = new PageInfo {ItemsCount = 3, Page = 1};

            InitializeCommands();
        }

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

        private void InitializeCommands()
        {
            _commandLoadEntities = new RelayCommand(LoadEntities, CanLoadEntities);
            _commandSaveEntities = new RelayCommand(SaveEntities, CanSaveEntities);
            _commandDeleteSelectedEntities = new RelayCommand(DeleteSelectedEntities, CanDeleteSelectedEntities);
            _commandCreateEntity = new RelayCommand(CreateEntity, CanCreateEntity);
            _commandEditEntity = new RelayCommand(EditEntity, CanEditEntity);
            _commandNextPage = new RelayCommand(NextPage,CanNextPage);
            _commandPreviousPage = new RelayCommand(PreviousPage, CanPreviousPage);
        }

        #region IEntitiesViewModel implementation
        protected virtual bool CanEditEntity(object obj)
        {
            return true;
        }

        protected virtual void EditEntity(object obj)
        {
            
        }

        protected virtual bool CanCreateEntity(object obj)
        {
            return true;
        }

        protected virtual bool CanDeleteSelectedEntities(object obj)
        {
            return true;
        }

        protected virtual bool CanSaveEntities(object obj)
        {
            return true;
        }

        protected virtual bool CanLoadEntities(object obj)
        {
            return true;
        }

        protected virtual void CreateEntity(object obj)
        {
            var item = new TEntity() {Id = Guid.NewGuid()};
            Entities.Add(item);
            SelectedEntity = item;
            EntitiesContext.Add(item);
        }

        protected virtual void DeleteSelectedEntities(object obj)
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

        protected virtual void SaveEntities(object obj)
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

        protected virtual void LoadEntities(object o)
        {
            Entities.Clear();
            var pageInfo = o as PageInfo;
            if (pageInfo != null)
            {
                var entities = EntitiesContext
                    .GetAll()
                    .Skip((pageInfo.Page - 1)*pageInfo.ItemsCount)
                    .Take(pageInfo.ItemsCount);
                Entities = new ObservableCollection<TEntity>(entities);
            }
            else
            {
                Entities = new ObservableCollection<TEntity>(EntitiesContext.GetAll());
            }
        }

        object IEntitiesViewModel.SelectedEntity
        {
            get { return SelectedEntity; }
        }

        public  ICommand CommandLoadEntities
        {
            get { return _commandLoadEntities; }
        }

        public  ICommand CommandSaveEntities
        {
            get { return _commandSaveEntities; }
        }

        public  ICommand CommandEditEntity
        {
            get { return _commandEditEntity; }
        }

        public  ICommand CommandUpdateEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        public  ICommand CommandDeleteEntity
        {
            get { return new RelayCommand(o => { }); }
        }

        public  ICommand CommandDeleteSelectedEntities
        {
            get { return _commandDeleteSelectedEntities; }
        }

        public  ICommand CommandDeleteAllEntities
        {
            get { return new RelayCommand(o => { }); }
        }

        public  ICommand CommandCreateEntity
        {
            get { return _commandCreateEntity; }
        }

        #endregion


        #region IPagedViewModel implementation

        public PageInfo PageInfo { get; set; }

        protected virtual bool CanPreviousPage(object obj)
        {
            return PageInfo.Page > 1;
        }

        protected virtual void PreviousPage(object obj)
        {
            PageInfo.Page--;
            LoadEntities(PageInfo);
        }

        protected virtual bool CanNextPage(object obj)
        {
            return Entities.Count >= PageInfo.ItemsCount;
        }

        protected virtual void NextPage(object obj)
        {
            PageInfo.Page++;
            LoadEntities(PageInfo);
        }

        public ICommand CommandNextPage
        {
            get { return _commandNextPage; }
        }

        public ICommand CommandPreviousPage
        {
            get { return _commandPreviousPage; }
        }
        #endregion
    }
}
