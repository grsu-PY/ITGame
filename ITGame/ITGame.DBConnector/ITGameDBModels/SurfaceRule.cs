namespace ITGame.DBConnector.ITGameDBModels
{
    using ITGame.Models.Environment;
    using ITGame.Models.Ñreature;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SurfaceRule")]
    public partial class SurfaceRule : ITGame.Models.Identity
    {
        [Key]
        public SurfaceType CurrentSurfaceType { get; set; }

        public HumanoidRaceType HumanoidRaceType { get; set; }

        public int HP { get; set; }

        public int MP { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Wisdom { get; set; }

        public int Constitution { get; set; }

        public virtual Surface Surface { get; set; }

        [Obsolete]
        [ScaffoldColumn(false)]
        public Guid Id
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        [Obsolete]
        [ScaffoldColumn(false)]
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
