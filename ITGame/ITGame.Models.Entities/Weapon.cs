using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Equipment;

namespace ITGame.Models.Entities
{
    [DataContract]
    [Table("Weapon")]
    public class Weapon : Identity, IViewModelItem
    {
        public Weapon()
        {
            HumanoidIds = new HashSet<Guid>();

            Humanoids = new HashSet<Humanoid>();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public WeaponType WeaponType { get; set; }

        [DataMember]
        public int PhysicalAttack { get; set; }

        [DataMember]
        public int MagicalAttack { get; set; }

        [DataMember]
        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        [DataMember]
        public int Weight { get; set; }

        [DataMember]
        public bool Equipped { get; set; }

        [DataMember]
        [Infrastructure.Data.ForeignKey(typeof(Humanoid), IsCollection = true)]
        public ICollection<Guid> HumanoidIds { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Humanoid> Humanoids { get; set; }

        #region IViewModelItem implementation

        [IgnoreDataMember]
        [ScaffoldColumn(false)]
        public bool IsSelectedModelItem { get; set; }

        #endregion
    }
}
