﻿using ITGame.Infrastructure.Data;
using ITGame.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ITGame.DBConnector
{
    public interface IEntityDbProjector<T> : IEntityProjector<T> where T : class, new()
    {
        DbSet<T> DbSet { get; }
        bool TryLoad(object id, out T value);
        T Load(object id);
    }

    public class EntityDbProjector<T> : IEntityDbProjector<T> where T : class, new()
    {
        private readonly DbContext _context;
        public EntityDbProjector(DbContext context)
        {
            _context = context;
            _context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }
        public DbSet<T> DbSet { get { return _context.Set<T>(); } }
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public T Create(IDictionary<string, string> values)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            try
            {
                T item = DbSet.Find(id);
                DbSet.Remove(item);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                DbSet.Remove(entity);
            }
            catch
            {
                // ignored
            }
        }

        public T Load(Guid id)
        {
            try
            {
                T item = DbSet.Find(id);
                return item;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool TryLoad(Guid id, out T value)
        {
            T item = null;
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

        public T Load(object id)
        {
            try
            {
                T item = DbSet.Find(id);
                return item;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool TryLoad(object id, out T value)
        {
            T item = null;
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

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void AddOrUpdate(T entity)
        {
            var entry = _context.Entry(entity);

            if (entry == null)
            {
                DbSet.Add(entity);
            }
            else switch (entry.State)
            {
                case EntityState.Added:
                    break;
                case EntityState.Detached:
                    entry.State = EntityState.Added;
                    break;
                default:
                    entry.State = EntityState.Modified;
                    break;
            }
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public IEnumerable<T> GetAll(Func<T, bool> where)
        {
            return DbSet.Where(where).ToList();
        }

        public Task<T> CreateAsync(IDictionary<string, string> values)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(T entity)
        {
            await AddAsync(entity, CancellationToken.None);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
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

        public async Task DeleteAsync(T entity)
        {
            await Task.Factory.StartNew(() => Delete(entity));
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.Factory.StartNew(() => Delete(id));
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Factory.StartNew(() => Update(entity));
        }

        public async Task<T> LoadAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<T, bool> where)
        {
            return await Task.Factory.StartNew(() => DbSet.ToListAsync().Result.Where(where));
        }
    }
}
