using ITGame.CLI.Models.Equipment;
using ITGame.CLI.Models.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ITGame.CLI.Models.Creature.Actions;

namespace ITGame.CLI.Models.Creature
{
    public class Humanoid : Creature, ICanEquip, ICanTake
    {
        protected Dictionary<string, Weapon> weapons;
        protected Dictionary<string, Armor> armors;
        protected Dictionary<string, Spell> spells;

        protected Helmet helmet;
        protected Body body;
        protected Gloves gloves;
        protected Boots boots;

        protected Weapon weapon;

        protected Spell spell;


        public void Equip(ITGame.CLI.Models.Equipment.Equipment equipment)
        {
            switch (equipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    weapon = equipment as Weapon;
                    break;
                case EquipmentType.Helmet:
                    helmet = equipment as Helmet;
                    break;
                case EquipmentType.Body:
                    body = equipment as Body;
                    break;
                case EquipmentType.Gloves:
                    gloves = equipment as Gloves;
                    break;
                case EquipmentType.Boots:
                    boots = equipment as Boots;
                    break;
                case EquipmentType.None:
                    break;
                default:
                    break;
            }
        }

        public void SelectSpell(Spell selectedSpell) {
            spell = selectedSpell;
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
                return base.MagicalAttack + (weapon != null ? weapon.MagicalAttack : spell.TotalMagicalAttack);
            }
        }

        public override void RecieveDamage(Damage damage)
        {
            var pDef = GetPDef();
            var mDef = GetMDef();

            damage.PhysicalDamage -= pDef;
            damage.MagicalDamage -= mDef;

            base.RecieveDamage(damage);
        }

        public virtual int GetPDef()
        {
            var pDef = helmet != null ? helmet.PhysicalDef : 0;
            pDef += gloves != null ? gloves.PhysicalDef : 0;
            pDef += boots != null ? boots.PhysicalDef : 0;
            pDef += body != null ? body.PhysicalDef : 0;
            return pDef;
        }

        public virtual int GetMDef()
        {
            var mDef = helmet != null ? helmet.MagicalDef : 0;
            mDef += gloves != null ? gloves.MagicalDef : 0;
            mDef += boots != null ? boots.MagicalDef : 0;
            mDef += body != null ? body.MagicalDef : 0;
            return mDef;
        }
    }
}
