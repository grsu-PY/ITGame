namespace ITGame.DBConnector.ITGameDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Surface")]
    public partial class Surface
    {
        [Key]
        public byte CurrentSurfaceType { get; set; }

        public byte HumanoidRaceType { get; set; }

        public virtual SurfaceRule SurfaceRule { get; set; }
    }
}
