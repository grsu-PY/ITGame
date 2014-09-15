using ITGame.CLI.Models.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Creature.Actions
{
    public interface IRecieveDamage
    {
        void RecieveDamage(Damage damage, AttackSpell targetAttackSpell = null);
    }
}
