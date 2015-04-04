using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Ñreature;

namespace ITGame.Models.Entities
{
    [DataContract]
    public class Humanoid : Identity
    {
        public Humanoid()
        {
            ArmorIds = new HashSet<Guid>();
            SpellIds = new HashSet<Guid>();
            WeaponIds = new HashSet<Guid>();

            Armors = new HashSet<Armor>();
            Spells = new HashSet<Spell>();
            Weapons = new HashSet<Weapon>();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember, ForeignKey(typeof(Character))]
        public Guid CharacterId { get; set; }

        [IgnoreDataMember]
        public Character Character { get; set; }

        [DataMember]
        public HumanoidRaceType HumanoidRaceType { get; set; }

        [DataMember]
        public int HP { get; set; }

        [DataMember]
        public int MP { get; set; }

        [DataMember]
        public int Strength { get; set; }

        [DataMember]
        public int Agility { get; set; }

        [DataMember]
        public int Wisdom { get; set; }

        [DataMember]
        public int Constitution { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public byte Level { get; set; }

        [DataMember]
        [ForeignKey(typeof(Armor), IsCollection = true)]
        public ICollection<Guid> ArmorIds { get; set; }

        [IgnoreDataMember]
        public ICollection<Armor> Armors { get; set; }

        [DataMember]
        [ForeignKey(typeof(Spell), IsCollection = true)]
        public ICollection<Guid> SpellIds { get; set; }

        [IgnoreDataMember]
        public ICollection<Spell> Spells { get; set; }

        [DataMember]
        [ForeignKey(typeof(Weapon), IsCollection = true)]
        public ICollection<Guid> WeaponIds { get; set; }

        [IgnoreDataMember]
        public ICollection<Weapon> Weapons { get; set; }
    }
}
