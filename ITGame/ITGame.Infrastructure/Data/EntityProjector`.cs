using ITGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGame.Infrastructure.Data
{
    public class EntityProjector<T> : IEntityProjector<T> where T : class, Identity, new()
    {
        private readonly IEntityProjector _ep;
        
        public EntityProjector(IEntityProjector ep)
        {
            _ep = ep;
        }
                
        public T Create(IDictionary<string, string> values)
        {
            var entity = _ep.Create(values) as T;

            return entity;
        }
        public async Task<T> CreateAsync(IDictionary<string, string> values)
        {
            return await _ep.CreateAsync(values) as T;
        }
        public void Add(T entity)
        {
            _ep.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _ep.AddAsync(entity);
        }
        public void SaveChanges()
        {
            _ep.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _ep.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _ep.Delete(entity);
        }
        public async Task DeleteAsync(T entity)
        {
            await _ep.DeleteAsync(entity);
        }

        public void Delete(Guid id)
        {
            _ep.Delete(id);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _ep.DeleteAsync(id);
        }
        public void Update(T entity)
        {
            _ep.Update(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            await _ep.UpdateAsync(entity);
        }
        public T Load(Guid id)
        {
            return _ep.Load(id) as T;
        }
        public async Task<T> LoadAsync(Guid id)
        {
            return await _ep.LoadAsync(id) as T;
        }

        public bool TryLoad(Guid id, out T value)
        {
            Identity entity;
            var result = _ep.TryLoad(id, out entity);
            if (result)
            {
                value = entity as T;
            }
            else
            {
                value = null;
            }
            return result;
        }

        public IEnumerable<T> GetAll()
        {
            return _ep.GetAll().Cast<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _ep.GetAllAsync();
            return result.Cast<T>();
        }
        public IEnumerable<T> GetAll(Func<T, bool> where)
        {
            return GetAll().Where(where);
        }
        public async Task<IEnumerable<T>> GetAllAsync(Func<T, bool> where)
        {
            var result = await GetAllAsync();
            return result.Where(where);
        }

    }
}
