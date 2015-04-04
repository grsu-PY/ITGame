using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using ITGame.Infrastructure.Extensions;

namespace ITGame.Infrastructure.Data
{
    public class EntityProjectorXml : EntityProjector
    {
        private const string EXTENSION = ".xml";
        private IEnumerable<Type> KnownTypes;

//        static EntityProjectorXml()
//        {
//            KnownTypes = TypeExtension.GetTypesFromModelAssembly();//.GetDataContractTypesFromModelsAssembly();
//        }

        public EntityProjectorXml(Type type, IEntityRepository repository) : base(type, repository)
        {
            KnownTypes = new[] {type};
        }

        protected override string Extension
        {
            get { return EXTENSION; }
        }

        protected override void InitTable()
        {
            Serialize(new EntitiesContainer(EntityType.FullName));
        }

        protected override IDictionary<Guid, Identity> LoadTable()
        {
            var entitiesWithEmptyForeignReferences = Deserialize<EntitiesContainer>().Entities;
            foreach (var identity in entitiesWithEmptyForeignReferences.Values)
            {
                LoadReferences(identity);
            }
            return entitiesWithEmptyForeignReferences;
        }

        private void LoadReferences(Identity identity)
        {
            try
            {
                var entityType = identity.GetType();
                var validProperties = entityType.GetSetGetProperties();
                foreach (var validProperty in validProperties)
                {
                    ProcessProperty(identity, validProperty);
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void ProcessProperty(object instance, PropertyInfo property)
        {
            var foreignKeyAttribute = property.GetCustomAttributes().OfType<ForeignKeyAttribute>().FirstOrDefault();
            if (foreignKeyAttribute != null)
            {
                SetForeignReference(instance, property, foreignKeyAttribute);
            }
        }

        private void SetForeignReference(object instance, PropertyInfo property, ForeignKeyAttribute foreignKeyAttribute)
        {
            var referenceType = foreignKeyAttribute.EntityType;
            if (foreignKeyAttribute.IsCollection)
            {
                SetCollectionReference(instance, property, foreignKeyAttribute, referenceType);
            }
            else
            {
                SetSingleReference(instance, property, foreignKeyAttribute, referenceType);
            }
        }

        private void SetCollectionReference(object instance, PropertyInfo property, ForeignKeyAttribute foreignKeyAttribute,
            Type referenceType)
        {
            var collectionOfRefsPropertyName = string.IsNullOrEmpty(foreignKeyAttribute.PropertyName)
                ? referenceType.Name + "s"
                : foreignKeyAttribute.PropertyName;

            var collectionOfRefsProp = instance.GetType().GetProperty(collectionOfRefsPropertyName);
            dynamic collectionOfIds = property.GetValue(instance);

            var openListType = typeof(List<>);
            var genericListType = openListType.MakeGenericType(referenceType);
            dynamic collectionOfRefs = Activator.CreateInstance(genericListType);

            if (collectionOfIds != null && collectionOfRefs != null)
            {
                var refRepo = Repository.GetInstance(referenceType);
                foreach (var id in collectionOfIds)
                {
                    Identity value;
                    if (refRepo.TryLoad((Guid) id, out value))
                    {
                        dynamic dynValue = value;
                        collectionOfRefs.Add(dynValue);
                    }
                }
                collectionOfRefsProp.SetValue(instance, collectionOfRefs);
            }

        }

        private void SetSingleReference(object instance, PropertyInfo property, ForeignKeyAttribute foreignKeyAttribute,
            Type referenceType)
        {
            var refPropertyName = string.IsNullOrEmpty(foreignKeyAttribute.PropertyName)
                ? referenceType.Name
                : foreignKeyAttribute.PropertyName;

            var refProperty = instance.GetType().GetProperty(refPropertyName);
            var idPropertyValue = property.GetValue(instance);

            var refRepo = Repository.GetInstance(referenceType);

            Identity value;
            if (refRepo.TryLoad((Guid) idPropertyValue, out value))
            {
                dynamic dynValue = value;
                refProperty.SetValue(instance, dynValue);
            }
        }

        public override void SaveChanges()
        {
            Serialize(EntitiesContainer);
        }

        private void Serialize(object obj)
        {
            using (var fileStream = new FileStream(TablePath, FileMode.Create))
            {
                var settings = new XmlWriterSettings()
                {
                    Indent = true,
                    ConformanceLevel = ConformanceLevel.Auto
                };
                using (var xmlWriter = XmlWriter.Create(fileStream, settings))
                {
                    using (var dictionaryWriter = XmlDictionaryWriter.CreateDictionaryWriter(xmlWriter))
                    {
                        var dataContractSerializer = new DataContractSerializer(obj.GetType(), KnownTypes);

                        dataContractSerializer.WriteObject(dictionaryWriter, obj);
                    }
                }

            }
        }
        
        private T Deserialize<T>()
        {
            T obj;
            using (var fileStream = new FileStream(TablePath, FileMode.Open))
            {
                using (var xmlReader = XmlReader.Create(fileStream))
                {
                    using (var dictionaryReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader))
                    {
                        var dataContractSerializer = new DataContractSerializer(typeof (T), KnownTypes);

                        obj = (T) dataContractSerializer.ReadObject(dictionaryReader);
                    }
                }

            }
            return obj;
        }
    }
}