using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;

namespace ITGame.Models.Entities
{
    [DataContract]
    public class Weapon : Identity
    {
        public Weapon()
        {
            HumanoidIds = new HashSet<Guid>();

            Humanoids = new HashSet<Humanoid>();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public byte WeaponType { get; set; }

        [DataMember]
        public int PhysicalAttack { get; set; }

        [DataMember]
        public int MagicalAttack { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Weight { get; set; }

        [DataMember]
        public bool Equipped { get; set; }

        [DataMember]
        [ForeignKey(typeof(Humanoid), IsCollection = true)]
        public ICollection<Guid> HumanoidIds { get; set; }

        [IgnoreDataMember]
        public ICollection<Humanoid> Humanoids { get; set; }
    }
}
