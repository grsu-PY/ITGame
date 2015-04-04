using ITGame.Infrastructure.Data;

namespace ITGame.DBConnector.ITGameDBModels
{
    using ITGame.Models.Equipment;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Armor")]
    public partial class Armor : Identity
    {
        public Armor()
        {
            Humanoids = new HashSet<Humanoid>();
        }

        public Guid Id { get; set; }

        public ArmorType ArmorType { get; set; }

        public int PhysicalDef { get; set; }

        public int MagicalDef { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public int Weight { get; set; }

        public bool Equipped { get; set; }

        public virtual ICollection<Humanoid> Humanoids { get; set; }
    }
}
