using System;
using System.Collections.Generic;

namespace ITGame.Models.Entities
{
    public class Weapon : Identity
    {
        public Weapon()
        {
            Humanoids = new HashSet<Guid>();
        }

        public Guid Id { get; set; }

        public byte WeaponType { get; set; }

        public int PhysicalAttack { get; set; }

        public int MagicalAttack { get; set; }

        public string Name { get; set; }

        public int Weight { get; set; }

        public bool Equipped { get; set; }

        public ICollection<Guid> Humanoids { get; set; }
    }
}
