using ITGame.CLI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITGame.CLI.Infrastructure
{
    public interface IEntityProjector
    {
        void Add(Identity entity);
        Identity Create(IDictionary<string, string> values);
        void Delete(Guid id);
        void Delete(Identity entity);
        Identity Load(Guid id);
        bool TryLoad(Guid id, out Identity value);
        void SaveChanges();
        void Update(Identity entity);

        IEnumerable<Identity> GetAll();
        IEnumerable<Identity> GetAll(Func<Identity, bool> where);


        Task<Identity> CreateAsync(IDictionary<string, string> values);
        Task AddAsync(Identity entity);
        Task SaveChangesAsync();
        Task DeleteAsync(Identity entity);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Identity entity);
        Task<Identity> LoadAsync(Guid id);
        Task<IEnumerable<Identity>> GetAllAsync();
        Task<IEnumerable<Identity>> GetAllAsync(Func<Identity, bool> where);
    }
}
