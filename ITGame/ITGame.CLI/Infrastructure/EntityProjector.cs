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
        private IDictionary<Guid, Identity> entities;
        private readonly Type EntityType;

        private readonly string tableName;
        private readonly string tablePath;

        public EntityProjector(Type type)
        {
            EntityType = type;
            entities = new Dictionary<Guid, Identity>();

            tableName = type.Name;

            tablePath = Path.Combine(EntityRepository.PATH, tableName + EntityRepository.EXT);

            if (!File.Exists(tablePath))
            {
                InitTable();
            }

            LoadTable();
        }
        private void InitTable()
        {
            var propertiesNames = EntityType
                .GetSetGetProperties()
                .Select(p => p.Name)
                .Aggregate((s1, s2) => s1 + EntityRepository.DELIM + s2);

            using (var writer = File.CreateText(tablePath))
            {
                writer.WriteLine(propertiesNames);
            };

        }

        private void LoadTable()
        {
            var rows = File.ReadAllLines(tablePath);
            var propNames = rows[0].Split(EntityRepository.DELIM.ToCharArray());

            for (int i = 1; i < rows.Length; i++)
            {
                var propValues = rows[i].Split(EntityRepository.DELIM.ToCharArray());
                var dict = new Dictionary<string, string>();

                for (int j = 0; j < propNames.Length && j < propValues.Length; j++)
                {
                    dict.Add(propNames[j], propValues[j]);
                }

                Identity entity = CreateEntity(dict) as Identity;

                if (!entities.ContainsKey(entity.Id))
                {
                    entities.Add(entity.Id, entity);
                }
                else
                {
                    throw new ApplicationException("Duplicated uniqueidentifier values");
                }
            }

        }
        private object CreateEntity(IDictionary<string, string> values)
        {
            object instance = Activator.CreateInstance(EntityType);

            var properties = EntityType
                .GetSetGetProperties()
                .Where(prop => values.ContainsKey(prop.Name));

            foreach (var property in properties)
            {
                if (property.PropertyType.IsEnum)
                {
                    property.SetValue(instance, Enum.Parse(property.PropertyType, values[property.Name]));
                }
                else if (property.PropertyType.IsClass || property.PropertyType.IsInterface)
                {
                    if (property.PropertyType.IsAssignableFrom(typeof(Identity)))
                    {
                        Guid id;
                        if (Guid.TryParse(values[property.Name], out id))
                        {
                            var propValue = EntityRepository.GetInstance(property.PropertyType).Load(id);
                            property.SetValue(instance, propValue);
                        }
                    }
                }
                else if (property.PropertyType.Equals(typeof(Guid)))
                {
                    Guid id;
                    if (Guid.TryParse(values[property.Name], out id))
                    {
                        property.SetValue(instance, id);
                    }
                    else
                    {
                        property.SetValue(instance, Guid.Empty);
                    }
                }
                else
                {
                    property.SetValue(instance, Convert.ChangeType(values[property.Name], property.PropertyType));
                }
            }

            return instance;
        }

        public void Add(Identity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                throw new ArgumentException("Duplicated uniqueidentifier values");
            }
            else
            {
                entities.Add(entity.Id, entity);
            }
        }

        public Identity Create(IDictionary<string, string> values)
        {
            var instance = CreateEntity(values) as Identity;

            return instance;
        }

        public void Delete(Guid id)
        {
            entities.Remove(id);
        }

        public void Delete(Identity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                Delete(entity.Id);
            }
        }

        public Identity Load(Guid id)
        {
            Identity entity;
            if (!entities.TryGetValue(id, out entity))
            {
                throw new ArgumentException(string.Format("There is no value with id {0}", id));
            }
            return entity;
        }

        public bool TryLoad(Guid id, out Identity value)
        {
            return entities.TryGetValue(id, out value);
        }

        public void SaveChanges()
        {

            StringBuilder table = new StringBuilder();
            StringBuilder row = new StringBuilder();
            string column = string.Empty;

            var properties = EntityType.GetSetGetProperties();

            var propertiesNames = properties
                .Select(p => p.Name)
                .Aggregate((s1, s2) => s1 + EntityRepository.DELIM + s2);

            table.AppendLine(propertiesNames);

            foreach (var entity in entities.Values)
            {
                row.Clear();

                foreach (var prop in properties)
                {
                    column = string.Empty;

                    if (prop.PropertyType.IsEnum)
                    {
                        column = Enum.GetName(prop.PropertyType, prop.GetValue(entity));
                    }
                    else if (prop.PropertyType.IsClass || prop.PropertyType.IsInterface)
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

            File.WriteAllText(tablePath, table.ToString());
        }

        public void Update(Identity entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                entities[entity.Id] = entity;
            }
            else
            {
                throw new ArgumentException(string.Format("Entity with uniqueidentifier {0} is not exist", entity.Id));
            }
        }

        public IEnumerable<Identity> GetAll()
        {
            return entities.Values;
        }

        public IEnumerable<Identity> GetAll(Func<Identity, bool> where)
        {
            return entities.Values.Where(where);
        }
    }

}
