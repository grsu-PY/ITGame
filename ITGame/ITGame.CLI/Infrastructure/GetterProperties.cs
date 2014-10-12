using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using ITGame.CLI.Extensions;
using ITGame.CLI.Models;

namespace ITGame.CLI.Infrastructure
{
    public static class GetterProperties
    {
        private static readonly string[] entities = { "Humanoid", "Weapon", "Armor", "Spell" };

        public static Hashtable GetProperties() 
        {
            Hashtable rTable = new Hashtable();

            foreach (string entity in entities) 
            {
                Type entityType = TypeExtension.GetTypeFromCurrentAssembly(entity);
                PropertyInfo[] properties = entityType.GetProperties();
                List<string> selectedProperties = new List<string>();

                foreach (PropertyInfo property in properties) 
                {
                    Attribute[] attributes = Attribute.GetCustomAttributes(property);

                    foreach (Attribute attribute in attributes) 
                    {
                        if (attribute is ParsingAttribute) 
                        {
                            selectedProperties.Add(property.Name);
                        }
                    }
                }

                rTable.Add(entity, selectedProperties);
            }

            return rTable;
        }

        public static List<string> GetProperties(string entity)
        {
            List<string> rList = new List<string>();

            Type entityType = TypeExtension.GetTypeFromCurrentAssembly(entity);
            PropertyInfo[] properties = entityType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(property);

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is ParsingAttribute)
                        rList.Add(property.Name);
                }
            }

            return rList;
        }

        public static int GetCountProperties(string entity) 
        {
            int rCount = 0;

            Type entityType = TypeExtension.GetTypeFromCurrentAssembly(entity);
            PropertyInfo[] properties = entityType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(property);

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is ParsingAttribute)
                        rCount++;
                }
            }

            return rCount;
        }
    }
}
