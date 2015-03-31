using ITGame.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ITGame.DBConnector
{
    public class EntityDBProjector
    {
        private readonly DbContext _context;
        private readonly Type _entityType;
        public EntityDBProjector(DbContext context, Type entityType)
        {
            _context = context;
            _context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }
        public DbSet DbSet { get { return _context.Set(_entityType); } }
        public void Add(object entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(Guid id)
        {
            try
            {
                object item = DbSet.Find(id);
                DbSet.Remove(item);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Delete(object entity)
        {
            try
            {
                DbSet.Remove(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public object Load(Guid id)
        {
            try
            {
                object item = DbSet.Find(id);
                return item;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool TryLoad(Guid id, out object value)
        {
            object item = null;
            try
            {
                item = DbSet.Find(id);
                return item != null;
            }
            catch (Exception)
            {
                return item != null;
            }
            finally
            {
                value = item;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(object entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void AddOrUpdate(object entity)
        {
            var entry = _context.Entry(entity);
            if (entry == null)
            {
                DbSet.Add(entity);
            }
            else
            {
                entry.State = EntityState.Modified;
            }
        }

        public IEnumerable<object> GetAll()
        {
            return new List<object>();
        }

        public IEnumerable<object> GetAll(Func<object, bool> where)
        {
            return new List<object>();
        }

        public Task<object> CreateAsync(IDictionary<string, string> values)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(object entity)
        {
            await AddAsync(entity, CancellationToken.None);
        }

        public async Task AddAsync(object entity, CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() => DbSet.Add(entity), cancellationToken);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(object entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(object entity)
        {
            throw new NotImplementedException();
        }

        public async Task<object> LoadAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<object>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public Task<IEnumerable<object>> GetAllAsync(Func<object, bool> where)
        {
            throw new NotImplementedException();
        }
    }
    
}
