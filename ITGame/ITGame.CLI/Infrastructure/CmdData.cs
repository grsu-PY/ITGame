using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ITGame.CLI.Infrastructure
{
    class CmdData
    {
        private readonly CmdCommands _Command;
        private readonly string _Entity;
        private readonly Guid _EntityGuid;
        private readonly Dictionary<string, string> _Properties;
        private readonly bool _IsGuid = false;
        public CmdData(CmdCommands command, string entity, Dictionary<string, string> props) 
        {
            this._Command = command;
            this._Entity = entity;
            this._Properties = props;
        }

        public CmdData(CmdCommands command, string entity, Guid entityGuid, Dictionary<string, string> props)
        {
            this._Command = command;
            this._Entity = entity;
            this._EntityGuid = entityGuid;
            this._Properties = props;
            this._IsGuid = true;
        }

        public CmdCommands Command 
        {
            get { return _Command; }
        }

        public string Entity
        {
            get { return _Entity; }
        }

        public Guid EntityGuid 
        {
            get { return _EntityGuid; }
        }

        public Dictionary<string, string> Properties
        {
            get { return _Properties; }
        }

        public bool IsGuid 
        {
            get { return _IsGuid; }
        }
    }
}
