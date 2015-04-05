using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Equipment;

namespace ITGame.Models.Entities
{
    [DataContract]
    public class Armor : Identity, IViewModelItem
    {
        public Armor()
        {
            HumanoidIds = new HashSet<Guid>();

            Humanoids = new HashSet<Humanoid>();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public ArmorType ArmorType { get; set; }

        [DataMember]
        public int PhysicalDef { get; set; }

        [DataMember]
        public int MagicalDef { get; set; }

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

        #region IViewModelItem implementation

        [IgnoreDataMember]
        public bool IsSelectedModelItem { get; set; }

        #endregion
    }
}
