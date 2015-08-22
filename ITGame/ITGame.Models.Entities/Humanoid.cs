using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Creature;

namespace ITGame.Models.Entities
{
    [DataContract]
    [Table("Humanoid")]
    public class Humanoid : Identity, IViewModelItem
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

        [DataMember, Infrastructure.Data.ForeignKey(typeof(Character))]
        public Guid CharacterId { get; set; }

        [IgnoreDataMember]
        public virtual Character Character { get; set; }

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
        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        [DataMember]
        [Range(1, byte.MaxValue)]
        public byte Level { get; set; }

        [DataMember]
        [Infrastructure.Data.ForeignKey(typeof(Armor), IsCollection = true)]
        public ICollection<Guid> ArmorIds { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Armor> Armors { get; set; }

        [DataMember]
        [Infrastructure.Data.ForeignKey(typeof(Spell), IsCollection = true)]
        public ICollection<Guid> SpellIds { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Spell> Spells { get; set; }

        [DataMember]
        [Infrastructure.Data.ForeignKey(typeof(Weapon), IsCollection = true)]
        public ICollection<Guid> WeaponIds { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Weapon> Weapons { get; set; }

        #region IViewModelItem implementation

        [IgnoreDataMember]
        [ScaffoldColumn(false)]
        public bool IsSelectedModelItem { get; set; }

        #endregion
    }
}
