using System;

namespace ITGame.Models.Magic
{
    public class Spell : Identity
    {
        /*
         * Fields
         */
        private SpellType spellType;
        private SchoolSpell schoolSpell;
        private int magicalPower;
        private int manaCost;
        private int totalDuration;
        private int currentDuration;

        /*
         * Methods
         */
        public Spell()
        {

        }

        /*
         * Properties
         */
        public Guid Id { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public SchoolSpell SchoolSpell
        {
            get { return schoolSpell; }
            set { schoolSpell = value; }
        }

        [Column]
        public SpellType SpellType 
        {
            get { return spellType; }
            set { spellType = value; }
        }

        [Column]
        public int MagicalPower
        {
            get { return magicalPower; }
            set { magicalPower = value; }
        }

        public float BaseSpeedCast  // Этот параметр пусть пока повисит, мало ли, решим реализовать
        {
            get { throw new NotImplementedException(); }
        }

        [Column]
        public int ManaCost
        {
            get { return manaCost; }
            set { manaCost = value; }
        }

        [Column]
        public int TotalDuration 
        {
            get { return totalDuration; }
            set { totalDuration = value; }
        }
        [Column]
        public int CurrentDuration 
        {
            get { return currentDuration; }
            set { currentDuration = value; }
        }


        public override string ToString()
        {
            //return string.Format("ID {0}, Name {1}, SpellType {2}, Power {3}, Mana Cost {4}",
            //    Id, Name, SchoolSpell, MagicalPower, ManaCost);

            return string.Format("Id -> {0}\n"+
                                 "Spell Type -> {1}\n"+
                                 "Power -> {2}\n"+
                                 "Mana Cost -> {3}",
                                 Id, SchoolSpell, MagicalPower, ManaCost);
        }
    }
}
