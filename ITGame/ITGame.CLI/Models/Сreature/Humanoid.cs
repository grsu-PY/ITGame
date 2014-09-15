using ITGame.CLI.Models.Creature.Actions;
using ITGame.CLI.Models.Equipment;
using ITGame.CLI.Models.Items;
using ITGame.CLI.Models.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Creature
{
    public class Humanoid : Creature, ICanEquip, ICanTake
    {
        protected Dictionary<Guid, Item> items;
        protected Dictionary<string, Weapon> weapons;
        protected Dictionary<string, Armor> armors;
        protected Dictionary<string, Spell> spells;

        protected Helmet helmet;
        protected Body body;
        protected Gloves gloves;
        protected Boots boots;

        protected Weapon weapon;

        protected AttackSpell attackSpell;
        protected DefensiveSpell defensiveSpell;
        
        public void Equip(ITGame.CLI.Models.Equipment.Equipment equipment)
        {
            switch (equipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    weapon = equipment as Weapon;
                    break;
                case EquipmentType.Helmet:
                    helmet = equipment as Helmet;
                    pDef += helmet.PhysicalDef;
                    mDef += helmet.MagicalDef;
                    break;
                case EquipmentType.Body:
                    body = equipment as Body;
                    pDef += body.PhysicalDef;
                    mDef += body.MagicalDef;
                    break;
                case EquipmentType.Gloves:
                    gloves = equipment as Gloves;
                    pDef += gloves.PhysicalDef;
                    mDef += gloves.MagicalDef;
                    break;
                case EquipmentType.Boots:
                    boots = equipment as Boots;
                    pDef += boots.PhysicalDef;
                    mDef += boots.MagicalDef;
                    break;
                case EquipmentType.None:
                    break;
                default:
                    break;
            }
        }
        
        public void RemoveEquipment(EquipmentType equipType)
        {
            switch (equipType)
            {
                case EquipmentType.Weapon:
                    weapon = null;
                    break;
                case EquipmentType.Helmet:
                    helmet = null;
                    pDef -= helmet.PhysicalDef;
                    mDef -= helmet.MagicalDef;
                    break;
                case EquipmentType.Body:
                    body = null;
                    pDef -= body.PhysicalDef;
                    mDef -= body.MagicalDef;
                    break;
                case EquipmentType.Gloves:
                    gloves = null;
                    pDef -= gloves.PhysicalDef;
                    mDef -= gloves.MagicalDef;
                    break;
                case EquipmentType.Boots:
                    boots = null;
                    pDef -= boots.PhysicalDef;
                    mDef -= boots.MagicalDef;
                    break;
                case EquipmentType.None:
                    break;
                default:
                    break;
            }
        }

        public void Take(Item item)
        {
            weight += item.Weight;
            if (weight > MaxWeight)
            {
                weight -= item.Weight;
                return;
            }

            items.Add(item.Id, item);
        }

        public void Drop(Guid itemId)
        {
            Item item;
            var exist = items.TryGetValue(itemId, out item);
            if (exist)
            {
                weight -= item.Weight;
                items.Remove(itemId);
            }            
        }

        public void SelectSpell(AttackSpell selectedAttackSpell = null, DefensiveSpell selectedDefensiveSpell = null) {
            this.attackSpell = selectedAttackSpell;
            this.defensiveSpell = selectedDefensiveSpell;
        }
        public override int PhysicalAttack
        {
            get
            {   // Предположительно, перед боем игроку будет дана возможность выбрать оружие и заклинание.
                // IsAttack - будет меняться на протяжении боя. Хотим ударить оружием меняем IsAttack на true. Ударили - возвращаем в false;
                // Аналогично с заклинаниями.
                // Данная модификация больше нужна для MagicalAttack.
                // В руке посох(маг урон) и заклинание.
                return base.PhysicalAttack + ((weapon != null && weapon.IsAttack != false) ? weapon.PhysicalAttack : 0);
            }
        }

        public override int MagicalAttack
        {
            get
            {
                return base.MagicalAttack + ((weapon != null && weapon.IsAttack != false) ? weapon.MagicalAttack : 
                                                             ((attackSpell != null && attackSpell.IsAttack != false) ? attackSpell.TotalMagicalAttack : 0));
            }
        }

        public override void RecieveDamage(Damage damage, SpellType spellType = SpellType.None)
        {
            var mDef = MagicalDefence;

            if (spellType != SpellType.None)
                mDef += (defensiveSpell != null &&
                        (defensiveSpell.IsAttack != false && defensiveSpell.SpellType == spellType)) ? defensiveSpell.TotalMagicalAttack : 0;

            damage.PhysicalDamage -= PhysicalDefence;
            damage.MagicalDamage -= mDef;

            base.RecieveDamage(damage);
        }

        public override void WeaponAttack()
        {
            if (_target == null) return;

            weapon.IsAttack = true;

            var message = string.Format("Your potential damage is {0}", PhysicalAttack);

            OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Fight));

            _target.RecieveDamage(new Damage
            {
                PhysicalDamage = PhysicalAttack,
                MagicalDamage = 0
            });

            weapon.IsAttack = false;
        }

        public override void SpellAttack()
        {
            if (_target == null) return;

            attackSpell.IsAttack = true;
            var message = string.Format("Your potential damage is {0}", MagicalAttack + attackSpell.TotalMagicalAttack);
            
            OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Fight));            

            _target.RecieveDamage(new Damage
            {
                PhysicalDamage = 0,
                MagicalDamage = MagicalAttack + attackSpell.TotalMagicalAttack
            }, attackSpell.SpellType);

            attackSpell.IsAttack = false;
        }

        protected override void OnActionPerformed(ActionPerformedEventArgs e)
        {
            base.OnActionPerformed(e);
        }
        
    }
}
