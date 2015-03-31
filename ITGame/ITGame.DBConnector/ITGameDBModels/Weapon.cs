namespace ITGame.DBConnector.ITGameDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Weapon")]
    public partial class Weapon : ITGame.Models.Identity
    {
        public Weapon()
        {
            Humanoids = new HashSet<Humanoid>();
        }

        public Guid Id { get; set; }

        public byte WeaponType { get; set; }

        public int PhysicalAttack { get; set; }

        public int MagicalAttack { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public int Weight { get; set; }

        public bool Equipped { get; set; }

        public virtual ICollection<Humanoid> Humanoids { get; set; }
    }
}
