namespace ITGame.DBConnector.ITGameDBModels
{
    using ITGame.Models.Environment;
    using ITGame.Models.Ñreature;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Surface")]
    public partial class Surface
    {
        [Key]
        public SurfaceType CurrentSurfaceType { get; set; }

        public HumanoidRaceType HumanoidRaceType { get; set; }
        
        public virtual SurfaceRule SurfaceRule { get; set; }
    }
}
