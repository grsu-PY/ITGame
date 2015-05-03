using System.Windows.Input;
using ITGame.DBManager.Commands;
using ITGame.DBManager.Navigations;
using ITGame.Infrastructure.Data;

namespace ITGame.DBManager.ViewModels
{
    public class EntityViewModel<TEntity> : BaseViewModel, IEntityViewModel where TEntity : class, Identity, new()
    {
        private readonly IEntityRepository _repository;
        private readonly IEntityProjector<TEntity> _entityContext;

        private ICommand _commandSave;
        private ICommand _commandCancel;
        private ICommand _commandDelete;

        public TEntity Entity { get; set; }

        public EntityViewModel(INavigation navigation, IEntityRepository repository, TEntity entity) : base(navigation)
        {
            _repository = repository;
            _entityContext = repository.GetInstance<TEntity>();
            Entity = entity;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _commandSave = new RelayCommand(Save, CanSave);
            _commandCancel = new RelayCommand(Cancel, CanCancel);
            _commandDelete = new RelayCommand(Delete, CanDelete);
        }

        protected virtual bool CanDelete(object obj)
        {
            return true;
        }

        protected virtual void Delete(object obj)
        {
            
        }

        protected virtual bool CanCancel(object obj)
        {
            return true;
        }

        protected virtual void Cancel(object obj)
        {

        }

        protected virtual bool CanSave(object obj)
        {
            return true;
        }

        protected virtual void Save(object obj)
        {

        }

        public object SelectedEntity
        {
            get { return Entity; }
        }

        public ICommand CommandSave
        {
            get { return _commandSave; }
        }

        public ICommand CommandCancel
        {
            get { return _commandCancel; }
        }

        public ICommand CommandDelete
        {
            get { return _commandDelete; }
        }

        public IEntityRepository Repository
        {
            get { return _repository; }
        }

        public IEntityProjector<TEntity> EntityContext
        {
            get { return _entityContext; }
        }
    }
}