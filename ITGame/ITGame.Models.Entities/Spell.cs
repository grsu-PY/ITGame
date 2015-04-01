using System;
using System.Collections.Generic;
using ITGame.Models.Magic;

namespace ITGame.Models.Entities
{
    public class Spell : Identity
    {
        public Spell()
        {
            Humanoids = new HashSet<Guid>();
        }

        public Guid Id { get; set; }

        public SpellType SpellType { get; set; }

        public SchoolSpell SchoolSpell { get; set; }

        public int MagicalPower { get; set; }

        public int ManaCost { get; set; }

        public int TotalDuration { get; set; }

        public string Name { get; set; }

        public bool Equipped { get; set; }

        public ICollection<Guid> Humanoids { get; set; }
    }
}
