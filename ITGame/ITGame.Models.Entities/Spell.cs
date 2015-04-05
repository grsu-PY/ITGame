using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Magic;

namespace ITGame.Models.Entities
{
    [DataContract]
    public class Spell : Identity, IViewModelItem
    {
        public Spell()
        {
            HumanoidIds = new HashSet<Guid>();

            Humanoids = new HashSet<Humanoid>();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public SpellType SpellType { get; set; }

        [DataMember]
        public SchoolSpell SchoolSpell { get; set; }

        [DataMember]
        public int MagicalPower { get; set; }

        [DataMember]
        public int ManaCost { get; set; }

        [DataMember]
        public int TotalDuration { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool Equipped { get; set; }

        [DataMember]
        [ForeignKey(typeof(Humanoid), IsCollection = true)]
        public ICollection<Guid> HumanoidIds { get; set; }

        [IgnoreDataMember]
        public ICollection<Humanoid> Humanoids { get; set; }

        #region IViewModelItem implementation

        [IgnoreDataMember]
        public bool IsSelectedModelItem { get; set; }

        #endregion
    }
}
