using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Creature;

namespace ITGame.Models.Entities
{
    [DataContract]
    [Table("HumanoidRace")]
    public class HumanoidRace : Identity, IViewModelItem
    {
        [DataMember]
        [Key]
        public HumanoidRaceType HumanoidRaceType { get; set; }

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
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        #region IViewModelItem implementation

        [IgnoreDataMember]
        [ScaffoldColumn(false)]
        public bool IsSelectedModelItem { get; set; }

        #endregion
    }
}
