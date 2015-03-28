using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using ITGame.Infrastructure.Extensions;
using ITGame.Models;
using ITGame.Models.Environment;

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
            return Deserialize<EntitiesContainer>().Entities;
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