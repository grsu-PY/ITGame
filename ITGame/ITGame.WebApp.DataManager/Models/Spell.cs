using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITGame.Infrastructure.Data;
using ITGame.Models.Entities;
using ITGame.Models.Magic;

namespace ITGame.WebApp.DataManager.Models
{
    public class Spell : Identity, IViewModelItem
    {
        public Spell()
        {
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

        
        public ICollection<Guid> HumanoidIds { get; set; }

        public virtual ICollection<Humanoid> Humanoids { get; set; }

        #region IViewModelItem implementation

        public bool IsSelectedModelItem { get; set; }

        #endregion
    }
}
