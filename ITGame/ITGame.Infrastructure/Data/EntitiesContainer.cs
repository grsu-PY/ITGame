using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.Infrastructure.Data
{
    [DataContract]
    public class EntitiesContainer
    {
        [DataMember]
        public string EntityType { get; private set; }

        [DataMember]
        public IDictionary<Guid, Identity> Entities
        {
            get { return _entities ?? (_entities = _lazyEntities.Value); }
            private set { _entities = value; }
        }

        private IDictionary<Guid, Identity> _entities;
        private readonly Lazy<IDictionary<Guid, Identity>> _lazyEntities;

        public EntitiesContainer(string entityType, Func<IDictionary<Guid, Identity>> dictionaryInitializer)
        {
            EntityType = entityType;
            _lazyEntities = new Lazy<IDictionary<Guid, Identity>>(dictionaryInitializer);
        }

        public EntitiesContainer(string entityType)
        {
            EntityType = entityType;
            Entities = new Dictionary<Guid, Identity>();
        }
    }
}
