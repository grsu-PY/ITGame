using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.CLI.Models.Magic
{
    class Spell
    {
        /*
         * Fields
         */
        private uint baseMagicalAttack;
        private uint bonusMagicalAttack;
        private int totalMagicalAttack;
        private string spellName;

        protected SpellType spellType;

        /*
         * Methods
         */
        public void Spell(string spellName, SpellType spellType, uint baseMagicalAttack, uint bonusMagicalAttack) {
            this.spellName = spellName;
            this.baseMagicalAttack = baseMagicalAttack;
            this.bonusMagicalAttack = bonusMagicalAttack;
            this.spellType = spellType;
            this.totalMagicalAttack = (int)baseMagicalAttack + (int)bonusMagicalAttack; // Временно, пока не решим вопрос, который я задал в VK
        }

        /*
         * Properties
         */
        public SpellType SpellType { get { return spellType; } }

        public string SpellName
        {
            get { return spellName; }
        }

        public uint BaseMagicalAttack
        {
            get { return baseMagicalAttack; }
        }

        public uint BonusMagicalAttack
        {
            get { return bonusMagicalAttack; }
            set { bonusMagicalAttack = value; }
        }
        public int TotalMagicalAttack
        {
            get { return totalMagicalAttack; }
            set { totalMagicalAttack = value; }
        }

        public float BaseSpeedCast  // Этот параметр пусть пока повисит, мало ли, решим реализовать
        {
            get;
        }

        public uint ManaCost
        {
            get;
        }

    }
}
