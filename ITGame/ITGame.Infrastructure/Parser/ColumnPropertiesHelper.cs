using ITGame.Infrastructure.Extensions;
using ITGame.Models;
using ITGame.Models.Equipment;
using ITGame.Models.Magic;
using ITGame.Models.Сreature;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ITGame.Infrastructure.Parser
{
    public static class ColumnPropertiesHelper
    {
        private static readonly Type[] entities = { typeof(Humanoid), typeof(Weapon), typeof(Armor), typeof(Spell) };

        public static Hashtable GetHashTable() 
        {
            Hashtable rTable = new Hashtable();

            foreach (Type entityType in entities) 
            {
                rTable.Add(entityType.Name, GetPropertiesNames(entityType));
            }

            return rTable;
        }

        public static IEnumerable<string> GetPropertiesNames(Type entityType)
        {
            return entityType.GetColumnProperties().Select(p => p.Name);
        }
        public static IEnumerable<string> GetPropertiesNames(string entity)
        {
            Type entityType = TypeExtension.GetTypeFromModelsAssembly(entity);

            return GetPropertiesNames(entityType);
        }

        public static int GetCountProperties(string entity)
        {
            Type entityType = TypeExtension.GetTypeFromModelsAssembly(entity);

            return entityType.GetColumnProperties().Count();
        }

        public static IEnumerable<PropertyInfo> GetColumnProperties(this Type type)
        {
            return type
                .GetProperties()
                .Where(property =>
                    Attribute.GetCustomAttributes(property).OfType<ColumnAttribute>().Any()
                    );
        }
    }
}
