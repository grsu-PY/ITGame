using ITGame.Infrastructure.Extensions;
using ITGame.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.Infrastructure.Data
{
    public class EntityProjector : IEntityProjector
    {
        #region Fields
        private const string EXSTENSION = ".csv";
        private readonly Type _entityType;

        private readonly object _lockObj = new object();

        private readonly string _tableName;
        private readonly string _tablePath;
        private readonly ObjectBuilder _objectBuilder;

        #endregion

        public EntityProjector(Type type, IEntityRepository repository)
        {
            lock (_lockObj)
            {
                _entityType = type;
                _objectBuilder = new ObjectBuilder(repository);

                EntitiesContainer = new EntitiesContainer(_entityType.FullName, InitializeEntities);

                _tableName = type.Name;

                _tablePath = Path.Combine(EntityRepository<EntityProjector>.PATH, _tableName + Extension);
            }
        }

        #region Properties
        protected virtual string Extension
        {
            get { return EXSTENSION; }
        }

        protected ObjectBuilder Builder
        {
            get { return _objectBuilder; }
        }

        protected IDictionary<Guid, Identity> Entities
        {
            get { return EntitiesContainer.Entities; }
        }

        protected Type EntityType
        {
            get { return _entityType; }
        }

        protected string TablePath
        {
            get { return _tablePath; }
        }

        protected EntitiesContainer EntitiesContainer { get; set; }

        #endregion

        private IDictionary<Guid, Identity> InitializeEntities()
        {
            if (!File.Exists(_tablePath))
            {
                InitTable();
            }

            return LoadTable();
        }
        
        protected virtual void InitTable()
        {
            var propertiesNames = _entityType
                .GetSetGetProperties()
                .Select(p => p.Name)
                .Aggregate((s1, s2) => s1 + EntityRepository<EntityProjector>.DELIM + s2);

            using (var writer = File.CreateText(_tablePath))
            {
                writer.WriteLine(propertiesNames);
            }
        }

        protected virtual IDictionary<Guid, Identity> LoadTable()
        {
            var localEntities = new Dictionary<Guid, Identity>();
            var rows = File.ReadAllLines(_tablePath);
            var propNames = rows[0].Split(EntityRepository<EntityProjector>.DELIM.ToCharArray());

            for (int i = 1; i < rows.Length; i++)
            {
                var propValues = rows[i].Split(EntityRepository<EntityProjector>.DELIM.ToCharArray());
                var dict = new Dictionary<string, string>();

                for (int j = 0; j < propNames.Length && j < propValues.Length; j++)
                {
                    dict.Add(propNames[j], propValues[j]);
                }

                var entity = CreateEntityInternal(dict) as Identity;

                if (entity != null && !localEntities.ContainsKey(entity.Id))
                {
                    localEntities.Add(entity.Id, entity);
                }
                else
                {
                    throw new ApplicationException("Duplicated uniqueidentifier values");
                }
            }
            return localEntities; 
        }

        protected virtual object CreateEntityInternal(IDictionary<string, string> values)
        {
            return Builder.CreateObject(_entityType, values);
        }

        public void Add(Identity entity)
        {
            
            if (EntitiesContainer.Entities.ContainsKey(entity.Id))
            {
                throw new ArgumentException("Duplicated uniqueidentifier values");
            }
            EntitiesContainer.Entities.Add(entity.Id, entity);
        }

        public Identity Create(IDictionary<string, string> values)
        {
            
            var instance = CreateEntityInternal(values) as Identity;

            return instance;
        }

        public void Delete(Guid id)
        {
            
            EntitiesContainer.Entities.Remove(id);
        }

        public void Delete(Identity entity)
        {
            
            if (EntitiesContainer.Entities.ContainsKey(entity.Id))
            {
                Delete(entity.Id);
            }
        }

        public Identity Load(Guid id)
        {
            
            Identity entity;
            if (!EntitiesContainer.Entities.TryGetValue(id, out entity))
            {
                throw new ArgumentException(string.Format("There is no value with id {0}", id));
            }
            return entity;
        }

        public bool TryLoad(Guid id, out Identity value)
        {
            return EntitiesContainer.Entities.TryGetValue(id, out value);
        }

        public virtual void SaveChanges()
        {
            

            var table = new StringBuilder();
            var row = new StringBuilder();

            var properties = _entityType.GetSetGetProperties();

            var propertiesNames = properties
                .Select(p => p.Name)
                .Aggregate((s1, s2) => s1 + EntityRepository<EntityProjector>.DELIM + s2);

            table.AppendLine(propertiesNames);

            foreach (var entity in EntitiesContainer.Entities.Values)
            {
                row.Clear();

                foreach (var prop in properties)
                {
                    var column = Builder.ToColumnValue(entity, prop);

                    row.Append(column);
                    row.Append(EntityRepository<EntityProjector>.DELIM);
                }
                row.Remove(row.Length - 1, 1);
                table.AppendLine(row.ToString());
            }

            File.WriteAllText(_tablePath, table.ToString());
        }

        public void Update(Identity entity)
        {            
            if (EntitiesContainer.Entities.ContainsKey(entity.Id))
            {
                EntitiesContainer.Entities[entity.Id] = entity;
            }
            else
            {
                throw new ArgumentException(string.Format("Entity with unique identifier {0} is not exist", entity.Id));
            }
        }

        public void AddOrUpdate(Identity entity)
        {
            if (EntitiesContainer.Entities.ContainsKey(entity.Id))
            {
                EntitiesContainer.Entities[entity.Id] = entity;
            }
            else
            {
                EntitiesContainer.Entities.Add(entity.Id, entity);
            }
        }

        public IEnumerable<Identity> GetAll()
        {
            
            return EntitiesContainer.Entities.Values;
        }

        public IEnumerable<Identity> GetAll(Func<Identity, bool> where)
        {
            
            return EntitiesContainer.Entities.Values.Where(where);
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
