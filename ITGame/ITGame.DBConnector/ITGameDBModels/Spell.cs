namespace ITGame.DBConnector.ITGameDBModels
{
    using ITGame.Models.Magic;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Spell")]
    public partial class Spell
    {
        public Spell()
        {
            Humanoids = new HashSet<Humanoid>();
        }

        public Guid Id { get; set; }

        public SpellType SpellType { get; set; }

        public SchoolSpell SchoolSpell { get; set; }

        public int MagicalPower { get; set; }

        public int ManaCost { get; set; }

        public int TotalDuration { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public bool Equipped { get; set; }

        public virtual ICollection<Humanoid> Humanoids { get; set; }
    }
}
