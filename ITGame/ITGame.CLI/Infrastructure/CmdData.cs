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
        private readonly string _Command;
        private readonly string _Entity;
        private readonly Dictionary<string, string> _Creature;
        private readonly Dictionary<string, string> _Weapon;
        private readonly Dictionary<string, string> _Armor;
        private readonly Dictionary<string, string> _Spell;

        public CmdData(Hashtable parsedData) 
        {
            this._Command = parsedData["Command"] as string;
            this._Entity = parsedData["Entity"] as string;
            this._Creature = parsedData["Creature"] as Dictionary<string, string>;
            this._Weapon = parsedData["Weapon"] as Dictionary<string, string>;
            this._Armor = parsedData["Armor"] as Dictionary<string, string>;
            this._Spell = parsedData["Spell"] as Dictionary<string, string>;
        }

        public string Command 
        {
            get { return _Command; }
        }

        public string Entity
        {
            get { return _Entity; }
        }

        public Dictionary<string, string> Creature
        {
            get { return _Creature; }
        }

        public Dictionary<string, string> Weapon
        {
            get { return _Weapon; }
        }

        public Dictionary<string, string> Armor
        {
            get { return _Armor; }
        }

        public Dictionary<string, string> Spell
        {
            get { return _Spell; }
        }
    }
}
