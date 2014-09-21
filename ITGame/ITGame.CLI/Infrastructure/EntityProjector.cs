using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ITGame.CLI.Models;
using System.IO;

namespace ITGame.CLI.Infrastructure
{
    public class EntityProjector<T> where T : class, Identity, new()
    {
        private IDictionary<Guid, T> entities;

        private readonly string path;
        private readonly string dbName;
        private readonly string tableName;
        private readonly string ext;
        private const string delim = ",";

        private const string DBFileExt = "DBFileExt";
        private const string DBLocation = "DBLocation";


        public EntityProjector()
        {
            var path = ConfigurationManager.AppSettings[DBLocation];
            ext = ConfigurationManager.AppSettings[DBFileExt];

            entities = new Dictionary<Guid, T>();

            tableName = typeof(T).Name;

            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
            {
                dbName = string.Empty;
                path = string.Empty;
            }
            else
            {
                dbName = Path.GetDirectoryName(path);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }

            this.path = Path.Combine(path, tableName + ext);

            if (!File.Exists(this.path))
            {
                InitTable();
            }

            LoadTable();
            //writer = TextWriter.Synchronized(new StreamWriter(this.path));
            //reader = TextReader.Synchronized(new StreamReader(this.path));
        }

        private void LoadTable()
        {
            var rows = File.ReadAllLines(path);
            var propNames = rows[0].Split(delim[0]);

            for (int i = 1; i < rows.Length; i++)
            {
                var propValues = rows[i].Split(delim[0]);
                var dict = new Dictionary<string, string>();

                for (int j = 0; j < propNames.Length && j < propValues.Length; j++)
                {
                    dict.Add(propNames[j], propValues[j]);
                }

                var entity = CreateEntity(dict);

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

        private void InitTable()
        {
            var propertiesNames = typeof(T)
                .GetProperties()
                .Where(p => p.GetSetMethod(false) != null)
                .Select(p => p.Name)
                .Aggregate((s1, s2) => s1 + delim + s2);

            using (var writer = File.CreateText(path))
            {
                writer.WriteLine(propertiesNames);
            };

        }

        public T Create(IDictionary<string, string> values)
        {
            var instance = CreateEntity(values);

            instance.Id = Guid.NewGuid();

            return instance;
        }

        private T CreateEntity(IDictionary<string, string> values)
        {
            Type entityType = typeof(T);

            T instance = Activator.CreateInstance<T>();

            var properties = entityType
                .GetProperties()
                .Where(p => p.GetSetMethod(false) != null)
                .Where(prop => values.ContainsKey(prop.Name));

            foreach (var property in properties)
            {
                if (property.PropertyType.IsEnum)
                {
                    property.SetValue(instance, Enum.Parse(property.PropertyType, values[property.Name]));
                }
                else if (property.PropertyType.IsClass || property.PropertyType.IsInterface)
                {

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

        public void Add(T entity)
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

        public void SaveChanges()
        {
            StringBuilder table = new StringBuilder();
            StringBuilder row = new StringBuilder();
            string column = string.Empty;

            var properties = typeof(T).GetProperties().Where(p => p.GetSetMethod(false) != null);

            var propertiesNames = properties
                .Select(p => p.Name)
                .Aggregate((s1, s2) => s1 + delim + s2);

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
                    else if (prop.PropertyType.IsClass)
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
                    row.Append(delim);
                }
                row.Remove(row.Length - 1, 1);
                table.AppendLine(row.ToString());
            }

            File.WriteAllText(path, table.ToString());
        }

        public void Delete(T entity)
        {
            if (entities.ContainsKey(entity.Id))
            {
                Delete(entity.Id);
            }
        }

        public void Delete(Guid id)
        {
            entities.Remove(id);
        }

        public void Update(T entity)
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

        public T Load(Guid id)
        {
            var properties = new Dictionary<string, string>();

            return CreateEntity(properties);
        }

    }

    public class IndentityComparer : IEqualityComparer<Identity>
    {
        public bool Equals(Identity x, Identity y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Identity obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
