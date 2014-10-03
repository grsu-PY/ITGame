using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.CLI.Models.Magic
{
    public class Spell : ITGame.CLI.Models.Identity
    {
        /*
         * Fields
         */
        private SpellType spellType;
        private SchoolSpell schoolSpell;
        private int magicalPower;
        private int manaCost;
        private string spellName;
        private bool isAttack;
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
        public string Name { get; set; }
        public SchoolSpell SchoolSpell
        {
            get { return schoolSpell; }
            set { schoolSpell = value; }
        }

        public SpellType SpellType 
        {
            get { return spellType; }
            set { spellType = value; }
        }

        public string SpellName
        {
            get { return spellName; }
            set { spellName = value; }
        }

        public int MagicalPower
        {
            get { return magicalPower; }
            set { magicalPower = value; }
        }

        public bool IsAttack {
            get { return isAttack; }
            set { isAttack = value; }
        }

        public float BaseSpeedCast  // Этот параметр пусть пока повисит, мало ли, решим реализовать
        {
            get { throw new NotImplementedException(); }
        }

        public int ManaCost
        {
            get { return manaCost; }
            set { manaCost = value; }
        }

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


        
    }
}
