using ITGame.CLI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ITGame.CLI.Extensions;
using System.Threading.Tasks;
using System.Threading;

namespace ITGame.CLI.Infrastructure
{
    public class EntityProjector : IEntityProjector
    {
        private readonly IDictionary<Guid, Identity> _entities;
        private readonly Type _entityType;

        private readonly object _lockObj = new object();

        private readonly string _tableName;
        private readonly string _tablePath;

        public EntityProjector(Type type)
        {
            lock (_lockObj)
            {
                _entityType = type;
                _entities = new Dictionary<Guid, Identity>();

                _tableName = type.Name;

                _tablePath = Path.Combine(EntityRepository.PATH, _tableName + EntityRepository.EXT);

                if (!File.Exists(_tablePath))
                {
                    InitTable();
                }

                LoadTable();
            }
        }
        private void InitTable()
        {
            var propertiesNames = _entityType
                .GetSetGetProperties()
                .Select(p => p.Name)
                .Aggregate((s1, s2) => s1 + EntityRepository.DELIM + s2);

            using (var writer = File.CreateText(_tablePath))
            {
                writer.WriteLine(propertiesNames);
            };

        }

        private void LoadTable()
        {
            var rows = File.ReadAllLines(_tablePath);
            var propNames = rows[0].Split(EntityRepository.DELIM.ToCharArray());

            for (int i = 1; i < rows.Length; i++)
            {
                var propValues = rows[i].Split(EntityRepository.DELIM.ToCharArray());
                var dict = new Dictionary<string, string>();

                for (int j = 0; j < propNames.Length && j < propValues.Length; j++)
                {
                    dict.Add(propNames[j], propValues[j]);
                }

                var entity = CreateEntity(dict) as Identity;

                if (entity != null && !_entities.ContainsKey(entity.Id))
                {
                    _entities.Add(entity.Id, entity);
                }
                else
                {
                    throw new ApplicationException("Duplicated uniqueidentifier values");
                }
            }
        }

        private object CreateEntity(IDictionary<string, string> values)
        {
            return ObjectBuilder.CreateObject( _entityType, values);
        }

        public void Add(Identity entity)
        {
            Thread.Sleep(2000);
            if (_entities.ContainsKey(entity.Id))
            {
                throw new ArgumentException("Duplicated uniqueidentifier values");
            }
            _entities.Add(entity.Id, entity);
        }

        public Identity Create(IDictionary<string, string> values)
        {
            Thread.Sleep(2000);
            var instance = CreateEntity(values) as Identity;

            return instance;
        }

        public void Delete(Guid id)
        {
            Thread.Sleep(2000);
            _entities.Remove(id);
        }

        public void Delete(Identity entity)
        {
            Thread.Sleep(2000);
            if (_entities.ContainsKey(entity.Id))
            {
                Delete(entity.Id);
            }
        }

        public Identity Load(Guid id)
        {
            Thread.Sleep(2000);
            Identity entity;
            if (!_entities.TryGetValue(id, out entity))
            {
                throw new ArgumentException(string.Format("There is no value with id {0}", id));
            }
            return entity;
        }

        public bool TryLoad(Guid id, out Identity value)
        {
            return _entities.TryGetValue(id, out value);
        }

        public void SaveChanges()
        {
            Thread.Sleep(2000);

            var table = new StringBuilder();
            var row = new StringBuilder();

            var properties = _entityType.GetColumnProperties().GetSetGetProperties();

            var propertiesNames = properties
                .Select(p => p.Name)
                .Aggregate((s1, s2) => s1 + EntityRepository.DELIM + s2);

            table.AppendLine(propertiesNames);

            foreach (var entity in _entities.Values)
            {
                row.Clear();

                foreach (var prop in properties)
                {
                    var column = ObjectBuilder.ToColumnValue(entity, prop);

                    row.Append(column);
                    row.Append(EntityRepository.DELIM);
                }
                row.Remove(row.Length - 1, 1);
                table.AppendLine(row.ToString());
            }

            File.WriteAllText(_tablePath, table.ToString());
        }

        public void Update(Identity entity)
        {
            Thread.Sleep(2000);
            if (_entities.ContainsKey(entity.Id))
            {
                _entities[entity.Id] = entity;
            }
            else
            {
                throw new ArgumentException(string.Format("Entity with uniqueidentifier {0} is not exist", entity.Id));
            }
        }

        public IEnumerable<Identity> GetAll()
        {
            Thread.Sleep(2000);
            return _entities.Values;
        }

        public IEnumerable<Identity> GetAll(Func<Identity, bool> where)
        {
            Thread.Sleep(2000);
            return _entities.Values.Where(where);
        }
        
        private T1 ThreadSafeFunc<T1>(Func<T1> func)
        {
            T1 returnValue = default(T1);
            lock (_lockObj)
            {
                returnValue = func();
            }
            return returnValue;
        }
        private void ThreadSafeAction(Action action)
        {
            lock (_lockObj)
            {
                action();
            }
        }
        
        public async Task<Identity> CreateAsync(IDictionary<string, string> values)
        {
            return 
                await Task.Run<Identity>(() =>
                    ThreadSafeFunc(() => Create(values))
                    );
        }

        public async Task AddAsync(Identity entity)
        {
            await Task.Run(() =>
               ThreadSafeAction(() => Add(entity))
               );
        }

        public async Task SaveChangesAsync()
        {
            await Task.Run(() =>
                ThreadSafeAction(() => SaveChanges())
                );
        }

        public async Task DeleteAsync(Identity entity)
        {
            await Task.Run(() =>
                ThreadSafeAction(() => Delete(entity))
                );
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.Run(() =>
                ThreadSafeAction(() => Delete(id))
                );
        }

        public async Task UpdateAsync(Identity entity)
        {
            await Task.Run(() =>
               ThreadSafeAction(() => Update(entity))
               );
        }

        public async Task<Identity> LoadAsync(Guid id)
        {
            return
                await Task.Run<Identity>(() =>
                    ThreadSafeFunc(() => Load(id))
                    );
        }

        public async Task<IEnumerable<Identity>> GetAllAsync()
        {
            return
              await Task.Run<IEnumerable<Identity>>(() =>
                  ThreadSafeFunc(() => GetAll())
                  );
        }

        public async Task<IEnumerable<Identity>> GetAllAsync(Func<Identity, bool> where)
        {
            return
               await Task.Run<IEnumerable<Identity>>(() =>
                   ThreadSafeFunc(() => GetAll(where))
                   );
        }
    }

}
