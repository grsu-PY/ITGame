using ITGame.CLI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITGame.CLI.Extensions;

namespace ITGame.CLI.Infrastructure
{
    public class EntityProjector : IEntityProjector
    {
        private readonly IDictionary<Guid, Identity> _entities;
        private readonly Type _entityType;

        private readonly string _tableName;
        private readonly string _tablePath;

        public EntityProjector(Type type)
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
            object instance = Activator.CreateInstance(_entityType);

            var properties = _entityType
                .GetSetGetProperties()
                .Where(prop => values.ContainsKey(prop.Name));

            foreach (var property in properties)
            {
                if (property.PropertyType.IsPrimitive)
                {
                     property.SetValue(instance, Convert.ChangeType(values[property.Name], property.PropertyType));
                }
                else if (property.PropertyType.IsEnum)
                {
                    property.SetValue(instance, Enum.Parse(property.PropertyType, values[property.Name]));
                }
                else if (property.PropertyType.IsAssignableFrom(typeof (Identity)))
                {
                    Guid id;
                    if (Guid.TryParse(values[property.Name], out id))
                    {
                        var propValue = EntityRepository.GetInstance(property.PropertyType).Load(id);
                        property.SetValue(instance, propValue);
                    }
                }
                else if (property.PropertyType == typeof (Guid))
                {
                    Guid id;
                    property.SetValue(instance, Guid.TryParse(values[property.Name], out id) ? id : Guid.Empty);
                }
                else
                {
                    try
                    {
                        property.SetValue(instance, Convert.ChangeType(values[property.Name], property.PropertyType));
                    }
                    catch
                    {
                        throw new Exception(
                            string.Format("Type {0} which is nested in {1} is not supported by EntityProjector",
                                property.PropertyType.Name, _entityType.Name));
                    }
                }
            }

            return instance;
        }

        public void Add(Identity entity)
        {
            if (_entities.ContainsKey(entity.Id))
            {
                throw new ArgumentException("Duplicated uniqueidentifier values");
            }
            _entities.Add(entity.Id, entity);
        }

        public Identity Create(IDictionary<string, string> values)
        {
            var instance = CreateEntity(values) as Identity;

            return instance;
        }

        public void Delete(Guid id)
        {
            _entities.Remove(id);
        }

        public void Delete(Identity entity)
        {
            if (_entities.ContainsKey(entity.Id))
            {
                Delete(entity.Id);
            }
        }

        public Identity Load(Guid id)
        {
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

            var table = new StringBuilder();
            var row = new StringBuilder();

            var properties = _entityType.GetSetGetProperties();

            var propertiesNames = properties
                .Select(p => p.Name)
                .Aggregate((s1, s2) => s1 + EntityRepository.DELIM + s2);

            table.AppendLine(propertiesNames);

            foreach (var entity in _entities.Values)
            {
                row.Clear();

                foreach (var prop in properties)
                {
                    var column = string.Empty;

                    if (prop.PropertyType.IsEnum)
                    {
                        column = Enum.GetName(prop.PropertyType, prop.GetValue(entity));
                    }
                    else if ((prop.PropertyType.IsClass || prop.PropertyType.IsInterface) && prop.PropertyType != typeof(string))
                    {
                        if (prop.PropertyType.IsAssignableFrom(typeof(Identity)))
                        {
                            column = Convert.ToString(prop.PropertyType.GetProperty("Id").GetValue(prop.GetValue(entity)));
                        }
                    }
                    else
                    {
                        column = Convert.ToString(prop.GetValue(entity));
                    }

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
            return _entities.Values;
        }

        public IEnumerable<Identity> GetAll(Func<Identity, bool> where)
        {
            return _entities.Values.Where(where);
        }
    }

}
