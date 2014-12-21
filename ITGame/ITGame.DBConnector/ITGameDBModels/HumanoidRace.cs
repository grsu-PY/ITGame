namespace ITGame.DBConnector.ITGameDBModels
{
    using ITGame.Models.Ñreature;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HumanoidRace")]
    public partial class HumanoidRace : ITGame.Models.Identity
    {
        [Key]
        public HumanoidRaceType HumanoidRaceType { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Wisdom { get; set; }

        public int Constitution { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

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
    }
}
