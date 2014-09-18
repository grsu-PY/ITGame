using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITGame.CLI.Models.Creature.Actions;
using ITGame.CLI.Models.Magic;

namespace ITGame.CLI.Models.Creature
{

    public abstract class Creature : IRecieveDamage, ICanAttack, IMoveable, Identity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        protected Creature _target;

        private int strength = 2;
        private int wisdom = 1;
        private int agility = 1;
        private int constitution = 5;
        
        private int initialMP = 5;
        private int initialHP = 20;

        protected int pDef;
        protected int mDef;

        protected int weight = 10;
        protected int speed = 10;

        private int currentMP;
        private int currentHP;

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
            set
            {
                strength = value;
            }
        }

        public int Wisdom
        {
            get
            {
                return wisdom;
            }
            set
            {
                wisdom = value;
            }
        }

        public int Constitution
        {
            get
            {
                return constitution;
            }
            set
            {
                constitution = value;
            }
        }

        public int Agility
        {
            get
            {
                return agility;
            }
            set
            {
                agility = value;
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

        public void ManaConsumption(int manaCost)
        {
            int mCost = manaCost <= currentMP ? manaCost : 0;

            MP -= mCost;
        }

        public virtual void RecieveDamage(Damage damage, SpellType spellType = SpellType.None)
        {
            var message = string.Format("You {0} have recieved {1} damage",
                Name,
                damage.MagicalDamage + damage.PhysicalDamage);

            HP -= damage.PhysicalDamage;
            HP -= damage.MagicalDamage;

            OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Fight));
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


        public virtual void SpellAttack()
        {
            throw new NotImplementedException();
        }

        public virtual void SpellDefense() {
            throw new NotImplementedException();
        }

        public event EventHandler<ActionPerformedEventArgs> ActionPerformed;

        protected virtual void OnActionPerformed(ActionPerformedEventArgs e)
        {
            EventHandler<ActionPerformedEventArgs> handler = ActionPerformed;

            if (handler != null)
            {
                handler(this, e);
            }
        }
        public virtual void SubscribeForSurface(ref EventHandler<SurfaceAffectEventArgs> ev)
        {
            ev += OnSurfaceChangedHandler;
        }

        protected abstract void OnSurfaceChangedHandler(object sender, SurfaceAffectEventArgs e);
    }
}
