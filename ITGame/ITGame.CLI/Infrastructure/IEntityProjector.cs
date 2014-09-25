using ITGame.CLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ITGame.CLI.Infrastructure
{
    public interface IEntityProjector<T>
     where T : class, new()
    {
        void Add(T entity);
        T Create(System.Collections.Generic.IDictionary<string, string> values);
        void Delete(Guid id);
        void Delete(T entity);
        T Load(Guid id);
        bool TryLoad(Guid id, out T value);
        void SaveChanges();
        void Update(T entity);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Func<T, bool> where);

    }

    public interface IEntityProjector
    {
        void Add(Identity entity);
        Identity Create(System.Collections.Generic.IDictionary<string, string> values);
        void Delete(Guid id);
        void Delete(Identity entity);
        Identity Load(Guid id);
        bool TryLoad(Guid id, out Identity value);
        void SaveChanges();
        void Update(Identity entity);

        IEnumerable<Identity> GetAll();
        IEnumerable<Identity> GetAll(Func<Identity, bool> where);
    }
}
