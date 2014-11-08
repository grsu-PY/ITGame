using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITGame.Infrastructure.Data
{
    public interface IEntityProjector<T>
     where T : class, new()
    {
        void Add(T entity);
        T Create(IDictionary<string, string> values);
        void Delete(Guid id);
        void Delete(T entity);
        T Load(Guid id);
        bool TryLoad(Guid id, out T value);
        void SaveChanges();
        void Update(T entity);

        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Func<T, bool> where);


        Task<T> CreateAsync(IDictionary<string, string> values);
        Task AddAsync(T entity);
        Task SaveChangesAsync();
        Task DeleteAsync(T entity);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(T entity);
        Task<T> LoadAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Func<T, bool> where);
    }
}
