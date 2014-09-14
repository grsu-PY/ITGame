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
        protected Dictionary<string, Item> items;
        protected Dictionary<string, Weapon> weapons;
        protected Dictionary<string, Armor> armors;
        protected Dictionary<string, Spell> spells;

        protected Helmet helmet;
        protected Body body;
        protected Gloves gloves;
        protected Boots boots;

        protected Weapon weapon;


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

            items.Add(item.Name, item);
        }

        public void Drop(Item item)
        {
            items.Remove(item.Name);

            weight -= item.Weight;
        }

        public void SelectSpell(AttackSpell selectedAttackSpell, DefensiveSpell selectedDefensiveSpell) {
            attackSpell = selectedAttackSpell;
            defensiveSpell = selectedDefensiveSpell;
        }
        public override int PhysicalAttack
        {
            get
            {
                return base.PhysicalAttack + (weapon != null ? weapon.PhysicalAttack : 0);
            }
        }

        public override int MagicalAttack
        {
            get
            {
                return base.MagicalAttack + (weapon != null ? weapon.MagicalAttack : attackSpell.TotalMagicalAttack);
            }
        }

        public override void RecieveDamage(Damage damage)
        {
            var mDef = GetMDef();

            damage.PhysicalDamage -= PhysicalDefence;
            damage.MagicalDamage -= mDef + this.mDef;

            base.RecieveDamage(damage);
        }
                

        public virtual int GetMDef()
        {
            mDef += (defensiveSpell != null && defensiveSpell.SpellType == _target.AttackSpellType) ? defensiveSpell.TotalMagicalAttack : 0;
            return mDef;
        }

    }
}
