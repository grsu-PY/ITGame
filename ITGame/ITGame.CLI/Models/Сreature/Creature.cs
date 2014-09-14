using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITGame.CLI.Models.Creature.Actions;
using ITGame.CLI.Models.Magic;

namespace ITGame.CLI.Models.Creature
{

    public abstract class Creature : IRecieveDamage, ICanAttack, IMoveable
    {
        protected Creature _target;
        private readonly int strength = 2;
        private readonly int wisdom = 1;
        private readonly int weight = 10;
        private readonly int agility = 1;
        private readonly int constitution = 20;
        private readonly int speed = 10;
        private readonly int maxMP = 5;
        private readonly int maxHP = 20;
        private int currentMP;
        private int currentHP;
        protected AttackSpell attackSpell;
        protected DefensiveSpell defensiveSpell;

        public Creature()
        {
            currentHP = maxHP;
            currentMP = maxMP;
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

        public int Weight
        {
            get
            {
                return weight + Constitution;
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
            }
        }

        public string Name { get; set; }

        public int HP
        {
            get
            {
                return currentHP;
            }
            set
            {
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

        public virtual void RecieveDamage(Damage damage)
        {
            int pDmg = damage.PhysicalDamage > 0 ? damage.PhysicalDamage : 0;
            int mDmg = damage.MagicalDamage > 0 ? damage.MagicalDamage : 0;

            HP -= pDmg;
            HP -= mDmg;
        }

        public virtual void Attack()
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
    }
}
