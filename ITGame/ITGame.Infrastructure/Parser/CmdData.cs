using System;
using System.Collections.Generic;

namespace ITGame.Infrastructure.Parser
{
    public class CmdData
    {
        private readonly CmdCommands _command;
        private readonly string _entityType;
        private readonly Guid _entityGuid;
        private readonly Dictionary<string, string> _properties;
        private readonly bool _isGuid = false;
        public CmdData(CmdCommands command, string entity, Dictionary<string, string> props) 
        {
            _command = command;
            _entityType = entity;
            _properties = props;
        }

        public CmdData(CmdCommands command, string entity, Guid entityGuid, Dictionary<string, string> props)
        {
            _command = command;
            _entityType = entity;
            _entityGuid = entityGuid;
            _properties = props;
            _isGuid = true;
        }

        public CmdCommands Command 
        {
            get { return _command; }
        }

        public string EntityType
        {
            get { return _entityType; }
        }

        public Guid EntityGuid 
        {
            get { return _entityGuid; }
        }

        public Dictionary<string, string> Properties
        {
            get { return _properties; }
        }

        public bool IsGuid 
        {
            get { return _isGuid; }
        }
    }
}
