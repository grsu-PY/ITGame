using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ITGame.CLI.Models;
using System.IO;

namespace ITGame.CLI.Infrastructure
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

        public void Add(T entity)
        {
            _ep.Add(entity);
        }

        public void SaveChanges()
        {
            _ep.SaveChanges();
        }

        public void Delete(T entity)
        {
            _ep.Delete(entity);
        }

        public void Delete(Guid id)
        {
            _ep.Delete(id);
        }

        public void Update(T entity)
        {
            _ep.Update(entity);
        }

        public T Load(Guid id)
        {
            return _ep.Load(id) as T;
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

        public IEnumerable<T> GetAll(Func<T, bool> where)
        {
            return GetAll().Where(where);
        }

    }
}
