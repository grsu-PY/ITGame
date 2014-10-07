using System;
using ITGame.CLI.Models.Environment;
using ITGame.CLI.Models.Magic;
using ITGame.CLI.Models.Сreature.Actions;

namespace ITGame.CLI.Models.Сreature
{

    public abstract class Creature : IRecieveDamage, ICanAttack, IMoveable, Identity
    {
        private string name;
        public Guid Id { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                var message = string.Format("New name - {0}", value);
                OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Info));
            }
        }

        protected Creature _target;
        protected SurfaceRule _lastSurfaceRule;
        protected SurfaceType _lastSurfaceType;

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

                var message = string.Format("The new value of health point - {0}", currentHP);
                OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Info));
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

                var message = string.Format("The new value of mana point - {0}", currentMP);
                OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Info));
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

        public virtual void RecieveDamage(Damage damage, SchoolSpell spellType = SchoolSpell.None)
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

        protected virtual void ApplySurfaceRule(SurfaceRule surfaceRule, bool bonus, double bonusRate = 1)
        {
            if (bonus)
            {
                Wisdom += (int)(surfaceRule.Wisdom * bonusRate);
                Strength += (int)(surfaceRule.Strength * bonusRate);
                Agility += (int)(surfaceRule.Agility * bonusRate);
                HP += (int)(surfaceRule.HP * bonusRate);
                MP += (int)(surfaceRule.MP * bonusRate);
            }
            else
            {
                Wisdom -= (int)(surfaceRule.Wisdom * bonusRate);
                Strength -= (int)(surfaceRule.Strength * bonusRate);
                Agility -= (int)(surfaceRule.Agility * bonusRate);
                HP -= (int)(surfaceRule.HP * bonusRate);
                MP -= (int)(surfaceRule.MP * bonusRate);
            }
        }
    }
}
