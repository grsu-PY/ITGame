using ITGame.CLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ITGame.CLI.Infrastructure
{
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
