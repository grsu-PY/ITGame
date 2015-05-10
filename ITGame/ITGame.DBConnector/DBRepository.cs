using ITGame.Infrastructure.Data;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace ITGame.DBConnector
{
    public class DbRepository : IEntityDbRepository
    {
        private DbContext _context;

        public DbRepository()
        {

        }

        public DbRepository(DbContext context)
        {
            _context = context;
        }

        public DbContext Context
        {
            get { return _context ?? (_context = new ITGameDBContext()); }
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        public IEntityProjector GetInstance(Type type)
        {
            throw new NotImplementedException();
        }

        public IEntityProjector<TEntity> GetInstance<TEntity>() where TEntity : class, Identity, new()
        {
            return new EntityDbProjector<TEntity>(Context);
        }

        public TCustomProjector GetInstance<TEntity, TCustomProjector>()
            where TEntity : class, Identity, new() 
            where TCustomProjector : IEntityProjector<TEntity>
        {
            throw new NotImplementedException();
        }

        public T RunInTransaction<T>(Func<T> func)
        {
            var returnValue = default(T);
            
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    returnValue = func();

                    Context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return returnValue;
        }

        public void RunInTransaction(Action action)
        {
            RunInTransaction(() =>
            {
                action();
                return 0;
            });
        }
        
        public void Dispose()
        {
            DisposeManagedResources();
        }

        private bool _disposed = false;
        protected virtual void DisposeManagedResources()
        {
            if(_disposed) return;

            if (Context != null)
            {
                Context.Dispose();
            }

            _disposed = true;
        }


    }
}
