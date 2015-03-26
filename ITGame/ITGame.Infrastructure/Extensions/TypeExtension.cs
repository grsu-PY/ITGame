using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

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
            return GetTypesFromModelAssembly().SingleOrDefault(t => string.Compare(t.Name, typeName, true) == 0);
        }
        public static Type GetTypeFromDBConnectorAssembly(string typeName)
        {
            return Assembly
                .LoadFrom("ITGame.DBConnector.dll")
                .GetTypes()
                .SingleOrDefault(t => string.Compare(t.Name, typeName, true) == 0);
        }

        public static IEnumerable<Type> GetTypesFromModelAssembly()
        {
            return Assembly.LoadFrom("ITGame.Models.dll").GetTypes();
        }

        public static IEnumerable<Type> GetDataContractTypesFromModelsAssembly()
        {
            return GetTypesFromModelAssembly().Where(x => x.GetCustomAttributes().OfType<DataContractAttribute>().Any());
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
