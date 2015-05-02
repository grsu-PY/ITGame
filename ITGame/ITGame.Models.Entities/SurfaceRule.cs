using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Environment;
using ITGame.Models.Ñreature;

namespace ITGame.Models.Entities
{

    [DataContract]
    [Table("SurfaceRule")]
    public class SurfaceRule : Identity, IViewModelItem
    {
        [DataMember]
        [Key]
        public SurfaceType CurrentSurfaceType { get; set; }

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

        [IgnoreDataMember]
        public virtual Surface Surface { get; set; }

        [DataMember,Infrastructure.Data.ForeignKey(typeof(Surface))]
        public SurfaceType SurfaceId { get; set; }

        [DataMember]
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [DataMember]
        [Required]
        [StringLength(40)]
        [ScaffoldColumn(false)]
        public string Name { get; set; }

        #region IViewModelItem implementation

        [IgnoreDataMember]
        [ScaffoldColumn(false)]
        public bool IsSelectedModelItem { get; set; }

        #endregion
    }
}
