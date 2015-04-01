using System;
using System.Collections.Generic;
using ITGame.Models.Equipment;

namespace ITGame.Models.Entities
{

    public class Armor : Identity
    {
        public Armor()
        {
            Humanoids = new HashSet<Guid>();
        }

        public Guid Id { get; set; }

        public ArmorType ArmorType { get; set; }

        public int PhysicalDef { get; set; }

        public int MagicalDef { get; set; }

        public string Name { get; set; }

        public int Weight { get; set; }

        public bool Equipped { get; set; }

        public ICollection<Guid> Humanoids { get; set; }
    }
}
