namespace ITGame.DBConnector.ITGameDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Character")]
    public partial class Character : ITGame.Models.Identity
    {
        public Character()
        {
            Humanoids = new HashSet<Humanoid>();
        }
        public Guid Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public byte Role { get; set; }

        public virtual ICollection<Humanoid> Humanoids { get; set; }
    }
}
