using System;
using System.Collections.Generic;
using System.Data;

namespace ITGame.Infrastructure.Parser
{
    public class GuiData
    {
        private readonly string _entityType;
        private readonly Dictionary<string, string> _properties;
        public GuiData(string entity, Dictionary<string, string> props)
        {
            _entityType = entity;
            _properties = props;
        }

        public string EntityType
        {
            get { return _entityType; }
        }

        public Dictionary<string, string> Properties
        {
            get { return _properties; }
        }
    }
}
