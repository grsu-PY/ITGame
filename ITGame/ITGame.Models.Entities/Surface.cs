using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Creature;
using ITGame.Models.Environment;

namespace ITGame.Models.Entities
{
    [DataContract]
    [Table("Surface")]
    public class Surface : Identity, IViewModelItem
    {
        [DataMember]
        [Key]
        public SurfaceType CurrentSurfaceType { get; set; }

        [DataMember]
        public HumanoidRaceType HumanoidRaceType { get; set; }

        [IgnoreDataMember]
        public virtual SurfaceRule SurfaceRule { get; set; }

        [DataMember, Infrastructure.Data.ForeignKey(typeof(SurfaceRule))]
        public Guid SurfaceRuleId { get; set; }

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
