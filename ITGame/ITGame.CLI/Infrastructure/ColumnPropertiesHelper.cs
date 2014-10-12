using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using ITGame.CLI.Extensions;
using ITGame.CLI.Models;
using ITGame.CLI.Models.Сreature;
using ITGame.CLI.Models.Equipment;
using ITGame.CLI.Models.Magic;

namespace ITGame.CLI.Infrastructure
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
            Type entityType = TypeExtension.GetTypeFromCurrentAssembly(entity);

            return GetPropertiesNames(entityType);
        }

        public static int GetCountProperties(string entity)
        {
            Type entityType = TypeExtension.GetTypeFromCurrentAssembly(entity);

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
