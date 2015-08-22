using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITGame.Infrastructure.Data;
using ITGame.Models.Entities;
using ITGame.Models.Equipment;

namespace ITGame.WebApp.DataManager.Models
{
    public class Armor : Identity
    {
        public Armor()
        {
            HumanoidIds = new HashSet<Guid>();

            Humanoids = new HashSet<Humanoid>();
        }

        public Guid Id { get; set; }

        [Display(Name = "Name name name")]
        public ArmorType ArmorType { get; set; }

        public int PhysicalDef { get; set; }

        public int MagicalDef { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Name name name")]
        public string Name { get; set; }

        public int Weight { get; set; }

        public bool Equipped { get; set; }

        public ICollection<Guid> HumanoidIds { get; set; }

        public virtual ICollection<Humanoid> Humanoids { get; set; }
    }
}
