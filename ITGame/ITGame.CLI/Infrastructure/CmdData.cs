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
        private readonly Dictionary<string, string> _Properties;
        public CmdData(CmdCommands command, string entity, Dictionary<string, string> props) 
        {
            this._Command = command;
            this._Entity = entity;
            this._Properties = props;
        }

        public CmdCommands Command 
        {
            get { return _Command; }
        }

        public string Entity
        {
            get { return _Entity; }
        }

        public Dictionary<string, string> Properties
        {
            get { return _Properties; }
        }
    }
}
