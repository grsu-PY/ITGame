using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITGame.CLI.Models.Creature.Actions;
using ITGame.CLI.Models.Magic;

namespace ITGame.CLI.Models.Creature
{

    public abstract class Creature : IRecieveDamage, ICanAttack, IMoveable, ITGame.CLI.Models.Identity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        protected Creature _target;

        private readonly int strength = 2;
        private readonly int wisdom = 1;
        private readonly int agility = 1;
        private readonly int constitution = 5;
        
        private readonly int initialMP = 5;
        private readonly int initialHP = 20;

        protected int pDef;
        protected int mDef;

        protected int weight = 10;
        protected int speed = 10;

        private int currentMP;
        private int currentHP;

        protected AttackSpell attackSpell;
        protected DefensiveSpell defensiveSpell;

        public Creature()
        {
            currentHP = MaxHP;
            currentMP = MaxMP;
            pDef = constitution;
            mDef = wisdom;

            Name = "Creature";
        }

        public int Strength
        {
            get
            {
                return strength;
            }
        }

        public int Wisdom
        {
            get
            {
                return wisdom;
            }
        }

        public int Constitution
        {
            get
            {
                return constitution;
            }
        }

        public int Agility
        {
            get
            {
                return agility;
            }
        }

        public int MaxWeight
        {
            get
            {
                return Constitution * 20;
            }
        }

        public int Speed
        {
            get
            {
                return speed + Agility;
            }
            protected set
            {
                speed = value;
            }
        }


        public int HP
        {
            get
            {
                return currentHP;
            }
            set
            {
                currentHP = value > 0
                    ? value > MaxHP ? MaxHP : value
                    : 0;
            }
        }

        public int MP
        {
            get
            {
                return currentMP;
            }
            set
            {
                currentMP = value > 0
                    ? value > MaxMP ? MaxMP : value
                    : 0;
            }
        }

        public SpellType AttackSpellType
        {
            get { return attackSpell.SpellType; }
        }

        public SpellType DefensiveSpellType
        {
            get { return defensiveSpell.SpellType; }
        }

        public virtual int PhysicalAttack
        {
            get
            {
                return Strength;
            }
        }

        public virtual int MagicalAttack
        {
            get
            {
                return Wisdom;
            }
        }

        public virtual int PhysicalDefence
        {
            get
            {
                return pDef;
            }
        }

        public virtual int MagicalDefence
        {
            get
            {
                return mDef;
            }
        }

        public virtual void RecieveDamage(Damage damage)
        {
            int pDmg = damage.PhysicalDamage > 0 ? damage.PhysicalDamage : 0;
            int mDmg = damage.MagicalDamage > 0 ? damage.MagicalDamage : 0;

            HP -= pDmg;
            HP -= mDmg;
        }

        public virtual void WeaponAttack()
        {
            if (_target == null) return;

            _target.RecieveDamage(new Damage
            {
                PhysicalDamage = PhysicalAttack,
                MagicalDamage = MagicalAttack
            });
        }

        public void SetTarget(Creature target)
        {
            _target = target;
        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public int MaxHP { get { return initialHP + constitution * 5; } }

        public int MaxMP { get { return initialMP + wisdom * 10; } }




        public void SpellAttack()
        {
            throw new NotImplementedException();
        }
    }
}
