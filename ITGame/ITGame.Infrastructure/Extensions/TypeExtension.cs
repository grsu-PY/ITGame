using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ITGame.Infrastructure.Extensions
{
    public static class TypeExtension
    {
        public static Type GetTypeFromCurrentAssembly(string typeName)
        {
            return Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .SingleOrDefault(t => string.Compare(t.Name, typeName, true) == 0);
        }
        public static Type GetTypeFromModelsAssembly(string typeName)
        {
            return Assembly
                .LoadFrom("ITGame.Models.dll")
                .GetTypes()
                .SingleOrDefault(t => string.Compare(t.Name, typeName, true) == 0);
        }
        public static IEnumerable<PropertyInfo> GetSetGetProperties(this Type type)
        {
            return type.GetProperties().GetSetGetProperties();
        }
        public static IEnumerable<PropertyInfo> GetSetGetProperties(this IEnumerable<PropertyInfo> properties)
        {
            return properties.Where(p => p.CanWrite && p.CanRead);
        }
    }
}
