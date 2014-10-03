using ITGame.CLI.Models.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Creature.Actions
{
    public interface IRecieveDamage : IAction
    {
        void RecieveDamage(Damage damage, SchoolSpell spellType = SchoolSpell.None);
    }
}
