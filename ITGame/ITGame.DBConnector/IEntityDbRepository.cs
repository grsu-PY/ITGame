using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using ITGame.Infrastructure.Data;

namespace ITGame.DBConnector
{
    public interface IEntityDbRepository : IEntityRepository, IDisposable
    {
        /// <summary>
        /// Function is executing within the transaction of the current DbContext. No need to execute SaveChanges
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        T RunInTransaction<T>(Func<T> func);

        /// <summary>
        /// Action is executing within the transaction of the current DbContext. No need to execute SaveChanges
        /// </summary>
        /// <param name="action"></param>
        void RunInTransaction(Action action);

        /// <summary>
        /// Get current DbContext
        /// </summary>
        DbContext Context { get; }

        void SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}