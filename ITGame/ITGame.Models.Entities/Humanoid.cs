using System;
using System.Collections.Generic;
using ITGame.Models.Ñreature;

namespace ITGame.Models.Entities
{
    public class Humanoid : Identity
    {
        public Humanoid()
        {
            Armors = new HashSet<Guid>();
            Spells = new HashSet<Guid>();
            Weapons = new HashSet<Guid>();
        }

        public Guid Id { get; set; }

        public Guid CharacterId { get; set; }
        
        public HumanoidRaceType HumanoidRaceType { get; set; }

        public int HP { get; set; }

        public int MP { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Wisdom { get; set; }

        public int Constitution { get; set; }

        public string Name { get; set; }

        public byte Level { get; set; }

        public Character Character { get; set; }

        public ICollection<Guid> Armors { get; set; }

        public ICollection<Guid> Spells { get; set; }

        public ICollection<Guid> Weapons { get; set; }
    }
}
