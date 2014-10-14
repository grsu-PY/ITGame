using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ITGame.CLI.Extensions;
using ITGame.CLI.Models;

namespace ITGame.CLI.Infrastructure
{
    internal static class ObjectBuilder
    {
        public static object CreateObject(Type type, IDictionary<string, string> values)
        {
            var instance = Activator.CreateInstance(type);

            var properties = type
                .GetSetGetProperties()
                .Where(prop => values.ContainsKey(prop.Name));

            foreach (var property in properties)
            {
                SetPropertyValueOf(instance, property, values[property.Name]);
            }

            return instance;
        }

        private static void SetPropertyValueOf(object instance, PropertyInfo property, string value)
        {
            object typedValue;
            var type = property.PropertyType;

            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                BuildDefault(type, out typedValue);
            }
            else
            if (type.IsEnum)
            {
                BuildEnum(type, value, out typedValue);
            }
            else if (typeof(Identity).IsAssignableFrom(type))
            {
                BuildIdentity(type, value, out typedValue);
            }
            else if (type == typeof(Guid))
            {
                typedValue = BuidlGuid(value);
            }
            else
            {
                try
                {
                    BuildPrimitive(type, value, out typedValue);
                }
                catch
                {
                    throw new Exception(
                        string.Format("Type {0} which is nested in {1} is not supported by EntityProjector",
                            type.Name, type.Name));
                }
            }

            if (typedValue != null)
            {
                property.SetValue(instance, typedValue);
            }
        }

        private static void BuildDefault(Type type, out object typedValue)
        {
            typedValue = type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private static Guid BuidlGuid(string value)
        {
            Guid id;
            if (!Guid.TryParse(value, out id))
            {
                id = Guid.Empty;
            }

            return id;
        }

        private static void BuildIdentity(Type type, string value, out object typedValue)
        {
            typedValue = EntityRepository.GetInstance(type).Load(BuidlGuid(value));
        }

        private static void BuildEnum(Type type, string value, out object typedValue)
        {
            typedValue = Enum.Parse(type, value);
        }

        private static void BuildPrimitive(Type type, string value, out object typedValue)
        {
            typedValue = Convert.ChangeType(value, type);
        }

        public static string ToColumnValue(object instance, PropertyInfo property)
        {
            string column;

            if (property.PropertyType.IsEnum)
            {
                column = Enum.GetName(property.PropertyType, property.GetValue(instance));
            }
            else if (property.PropertyType.IsAssignableFrom(typeof(Identity)))
            {
                column = Convert.ToString(property.PropertyType.GetProperty("Id").GetValue(property.GetValue(instance)));
            }
            else
            {
                column = Convert.ToString(property.GetValue(instance));
            }

            return column;
        }

    }
}
