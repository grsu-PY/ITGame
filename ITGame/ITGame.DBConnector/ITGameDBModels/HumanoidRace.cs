namespace ITGame.DBConnector.ITGameDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HumanoidRace")]
    public partial class HumanoidRace
    {
        public HumanoidRace()
        {
            Humanoid = new HashSet<Humanoid>();
        }

        [Key]
        public byte HumanoidRaceType { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Wisdom { get; set; }

        public int Constitution { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public virtual ICollection<Humanoid> Humanoid { get; set; }
    }
}
