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
        private int baseMagicalAttack;
        private int bonusMagicalAttack;
        private int totalMagicalAttack;
        private int manaCost;
        private string spellName;
        private bool isAttack;

        protected SpellType spellType;

        /*
         * Methods
         */
        public Spell()
        {

        }
        public Spell(string spellName, SpellType spellType, int baseMagicalAttack, int bonusMagicalAttack, int manaCost) {
            this.spellName = spellName;
            this.baseMagicalAttack = baseMagicalAttack;
            this.bonusMagicalAttack = bonusMagicalAttack;
            this.manaCost = manaCost;
            this.spellType = spellType;
            this.totalMagicalAttack = baseMagicalAttack + bonusMagicalAttack;
        }

        /*
         * Properties
         */
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SpellType SpellType { get { return spellType; } }

        public string SpellName
        {
            get { return spellName; }
        }

        public int BaseMagicalAttack
        {
            get { return baseMagicalAttack; }
        }

        public int BonusMagicalAttack
        {
            get { return bonusMagicalAttack; }
            set { bonusMagicalAttack = value; }
        }
        public int TotalMagicalAttack
        {
            get { return totalMagicalAttack; }
            set { totalMagicalAttack = value; }
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
        }


        
    }
}
