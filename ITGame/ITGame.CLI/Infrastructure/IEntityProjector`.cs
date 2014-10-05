using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
