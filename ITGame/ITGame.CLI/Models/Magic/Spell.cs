using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.CLI.Models.Magic
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

        [ParsingAttribute]
        public string Name { get; set; }

        [ParsingAttribute]
        public SchoolSpell SchoolSpell
        {
            get { return schoolSpell; }
            set { schoolSpell = value; }
        }

        [ParsingAttribute]
        public SpellType SpellType 
        {
            get { return spellType; }
            set { spellType = value; }
        }

        [ParsingAttribute]
        public int MagicalPower
        {
            get { return magicalPower; }
            set { magicalPower = value; }
        }

        public float BaseSpeedCast  // Этот параметр пусть пока повисит, мало ли, решим реализовать
        {
            get { throw new NotImplementedException(); }
        }

        [ParsingAttribute]
        public int ManaCost
        {
            get { return manaCost; }
            set { manaCost = value; }
        }

        [ParsingAttribute]
        public int TotalDuration 
        {
            get { return totalDuration; }
            set { totalDuration = value; }
        }

        public int CurrentDuration 
        {
            get { return currentDuration; }
            set { currentDuration = value; }
        }


        public override string ToString()
        {
            return string.Format("ID {0}, Name {1}, SpellType {2}, Power {3}, Mana Cost {4}",
                Id, Name, SchoolSpell, MagicalPower, ManaCost);
        }
    }
}
