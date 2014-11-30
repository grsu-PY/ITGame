namespace ITGame.DBConnector.ITGameDBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Humanoid")]
    public partial class Humanoid
    {
        public Humanoid()
        {
            Armors = new HashSet<Armor>();
            Spells = new HashSet<Spell>();
            Weapon = new HashSet<Weapon>();
        }

        public Guid Id { get; set; }

        public byte HumanoidRaceType { get; set; }

        public int HP { get; set; }

        public int MP { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Wisdom { get; set; }

        public int Constitution { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public byte Level { get; set; }

        public virtual Character Character { get; set; }

        public virtual HumanoidRace HumanoidRace { get; set; }

        public virtual ICollection<Armor> Armors { get; set; }

        public virtual ICollection<Spell> Spells { get; set; }

        public virtual ICollection<Weapon> Weapon { get; set; }
    }
}
