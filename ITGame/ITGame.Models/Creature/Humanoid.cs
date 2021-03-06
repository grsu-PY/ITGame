﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Creature.Actions;
using ITGame.Models.Environment;
using ITGame.Models.Equipment;
using ITGame.Models.Items;
using ITGame.Models.Magic;

namespace ITGame.Models.Creature
{
    [DataContract]
    public class Humanoid : Creature, ICanEquip, ICanTake
    {
        protected HumanoidRaceType humanoidRace;

        protected readonly List<Item> items;
        protected readonly List<Weapon> weapons;
        protected readonly List<Armor> armors;
        protected readonly List<Spell> spells;

        protected Armor body;
        protected Armor boots;
        protected Armor gloves;
        protected Armor helmet;

        protected Weapon weapon;

        protected Spell attackSpell;
        protected Spell defensiveSpell;

        public Humanoid()
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            spells = new List<Spell>();
            items = new List<Item>();
        }

        public void Equip(Equipment.Equipment equipment)
        {
            switch (equipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    weapon = equipment as Weapon;
                    weapons.Add(weapon);
                    break;
                case EquipmentType.Armor:
                    var armor = equipment as Armor;
                    switch (armor.ArmorType)
                    {
                        case ArmorType.Body:
                            body = armor;
                            armors.Add(armor);
                            pDef += body.PhysicalDef;
                            mDef += body.MagicalDef;
                            break;
                        case ArmorType.Boots:
                            boots = armor;
                            armors.Add(armor);
                            pDef += boots.PhysicalDef;
                            mDef += boots.MagicalDef;
                            break;
                        case ArmorType.Gloves:
                            gloves = armor;
                            armors.Add(armor);
                            pDef += gloves.PhysicalDef;
                            mDef += gloves.MagicalDef;
                            break;
                        case ArmorType.Helmet:
                            helmet = armor;
                            armors.Add(armor);
                            pDef += helmet.PhysicalDef;
                            mDef += helmet.MagicalDef;
                            break;
                        case ArmorType.None:
                            break;
                        default:
                            break;
                    }
                    break;
                case EquipmentType.None:
                    break;
                default:
                    break;
            }
        }

        public void RemoveEquipment(Equipment.Equipment equipment)
        {
            switch (equipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    weapon = null;
                    break;
                case EquipmentType.Armor:
                    var armorType = (equipment as Armor).ArmorType;
                    switch (armorType)
                    {
                        case ArmorType.Body:
                            pDef -= body.PhysicalDef;
                            mDef -= body.MagicalDef;
                            body = null;
                            break;
                        case ArmorType.Boots:
                            pDef -= boots.PhysicalDef;
                            mDef -= boots.MagicalDef;
                            boots = null;
                            break;
                        case ArmorType.Gloves:
                            pDef -= gloves.PhysicalDef;
                            mDef -= gloves.MagicalDef;
                            gloves = null;
                            break;
                        case ArmorType.Helmet:
                            pDef -= helmet.PhysicalDef;
                            mDef -= helmet.MagicalDef;
                            helmet = null;
                            break;
                        case ArmorType.None:
                            break;
                        default:
                            break;
                    }
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

            items.Add(item);
        }

        public void Drop(Guid itemId)
        {
            var item = items.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                weight -= item.Weight;
                items.Remove(item);
            }
        }

        public void SelectSpell(Spell selectedAttackSpell = null, Spell selectedDefensiveSpell = null)
        {
            attackSpell = selectedAttackSpell;
            defensiveSpell = selectedDefensiveSpell;
        }

        [Column]
        public HumanoidRaceType HumanoidRace
        {
            get
            {
                return humanoidRace;
            }
            set
            {
                humanoidRace = value;

                var message = string.Format("Humanoid race changed to {0}", value);
                OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Info));
            }
        }

        public override int PhysicalAttack
        {
            get
            {
                return base.PhysicalAttack + ((weapon != null) ? weapon.PhysicalAttack : 0);
            }
        }

        public override int MagicalAttack
        {
            get
            {
                return base.MagicalAttack + ((weapon != null) ? weapon.MagicalAttack :
                                                             ((attackSpell != null) ? attackSpell.MagicalPower : 0));
            }
        }

        public override void RecieveDamage(Damage damage, SchoolSpell schoolSpell = SchoolSpell.None)
        {
            var mDef = MagicalDefence;

            if (schoolSpell != SchoolSpell.None)
                mDef += (defensiveSpell != null &&
                        (defensiveSpell.CurrentDuration != 0 && defensiveSpell.SchoolSpell == schoolSpell)) ? defensiveSpell.MagicalPower : 0;

            if (defensiveSpell != null)
                defensiveSpell.CurrentDuration -= defensiveSpell.CurrentDuration != 0 ? 1 : 0;

            damage.PhysicalDamage -= pDef;
            damage.MagicalDamage -= mDef;

            base.RecieveDamage(damage);
        }

        public override void WeaponAttack()
        {
            if (_target == null || weapon == null) return;

            var message = string.Format("Your potential damage is {0}", PhysicalAttack);

            OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Fight));

            _target.RecieveDamage(new Damage
            {
                PhysicalDamage = PhysicalAttack,
                MagicalDamage = MagicalAttack
            });
        }

        public override void SpellAttack()
        {
            if (_target == null || attackSpell == null) return;

            var message = "";
            if (attackSpell.ManaCost > MP)
            {
                message = "You have not a mana point";
                OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Info));
                return;
            }

            message = string.Format("Your potential damage is {0}", MagicalAttack);

            OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Fight));

            _target.RecieveDamage(new Damage
            {
                PhysicalDamage = 0,
                MagicalDamage = MagicalAttack
            }, attackSpell.SchoolSpell);

            ManaConsumption(attackSpell.ManaCost);
        }

        public override void SpellDefense()
        {
            if (defensiveSpell == null) return;

            if (defensiveSpell.ManaCost > MP)
            {
                var message = "You have not a mana point";
                OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Info));
                return;
            }

            defensiveSpell.CurrentDuration = defensiveSpell.TotalDuration;
            ManaConsumption(defensiveSpell.ManaCost);
        }

        [Column]
        [DataMember(Name = "Weapon")]
        public Guid WeaponID { get; set; }
        [DataMember(Name = "AttackSpell")]
        public Guid AttackSpellID { get; set; }
        [DataMember(Name = "DefensiveSpell")]
        public Guid DefensiveSpellID { get; set; }

        [IgnoreDataMember]
        public Weapon Weapon { get { return weapon; } set { weapon = value; } }
        [IgnoreDataMember]
        public Spell AttackSpell { get { return attackSpell; } set { attackSpell = value; } }
        [IgnoreDataMember]
        public Spell DefensiveSpell { get { return defensiveSpell; } set { defensiveSpell = value; } }

        public IEnumerable<Weapon> Weapons
        {
            get { return weapons; }
        }

        public IEnumerable<Armor> Armors
        {
            get { return armors; }
        }

        public IEnumerable<Spell> Spells
        {
            get { return spells; }
        }

        protected override void OnActionPerformed(ActionPerformedEventArgs e)
        {
            base.OnActionPerformed(e);
        }

        protected override void OnSurfaceChangedHandler(object sender, SurfaceAffectEventArgs e)
        {
            if (e.surfaceType == _lastSurfaceType) return;//могут ли сюда прийти новые правила для старой поверхности?

            OnActionPerformed(new ActionPerformedEventArgs(
                string.Format("Oh, look at this, {0}. Surface was changed to {1}", Name, e.surfaceType),
                ActionType.Info)
                );

            RemoveOldSurfaceBonus();

            _lastSurfaceRule = e.actionRule;
            _lastSurfaceType = e.surfaceType;

            SetNewSurfaceRule(e);
        }

        private void SetNewSurfaceRule(SurfaceAffectEventArgs e)
        {
            switch (HumanoidRace)
            {
                case HumanoidRaceType.Dwarf:
                    switch (e.surfaceType)
                    {
                        case SurfaceType.Ground:
                            ApplySurfaceRule(e.actionRule, bonus: true, bonusRate: 1.2);
                            break;
                        case SurfaceType.Water:
                            ApplySurfaceRule(e.actionRule, bonus: true, bonusRate: 0.2);
                            break;
                        case SurfaceType.Lava:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 0.8);
                            break;
                        case SurfaceType.Swamp:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 0.6);
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRaceType.Elf:
                    switch (e.surfaceType)
                    {
                        case SurfaceType.Ground:
                            ApplySurfaceRule(e.actionRule, bonus: true);
                            break;
                        case SurfaceType.Water:
                            ApplySurfaceRule(e.actionRule, bonus: true);
                            break;
                        case SurfaceType.Lava:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 1.5);
                            break;
                        case SurfaceType.Swamp:
                            ApplySurfaceRule(e.actionRule, bonus: false);
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRaceType.Human:
                    switch (e.surfaceType)
                    {
                        case SurfaceType.Ground:
                            ApplySurfaceRule(e.actionRule, bonus: true);
                            break;
                        case SurfaceType.Water:
                            ApplySurfaceRule(e.actionRule, bonus: true, bonusRate: 0.5);
                            break;
                        case SurfaceType.Lava:
                            ApplySurfaceRule(e.actionRule, bonus: false);
                            break;
                        case SurfaceType.Swamp:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 0.5);
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRaceType.Orc:
                    switch (e.surfaceType)
                    {
                        case SurfaceType.Ground:
                            ApplySurfaceRule(e.actionRule, bonus: true);
                            break;
                        case SurfaceType.Water:
                            ApplySurfaceRule(e.actionRule, bonus: false);
                            break;
                        case SurfaceType.Lava:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 0.8);
                            break;
                        case SurfaceType.Swamp:
                            ApplySurfaceRule(e.actionRule, bonus: true);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void RemoveOldSurfaceBonus()
        {
            switch (HumanoidRace)
            {
                case HumanoidRaceType.Dwarf:
                    switch (_lastSurfaceType)
                    {
                        case SurfaceType.Ground:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false, bonusRate: 1.2);
                            break;
                        case SurfaceType.Water:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false, bonusRate: 0.2);
                            break;
                        case SurfaceType.Lava:
                            break;
                        case SurfaceType.Swamp:
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRaceType.Elf:
                    switch (_lastSurfaceType)
                    {
                        case SurfaceType.Ground:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false);
                            break;
                        case SurfaceType.Water:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false);
                            break;
                        case SurfaceType.Lava:
                            break;
                        case SurfaceType.Swamp:
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRaceType.Human:
                    switch (_lastSurfaceType)
                    {
                        case SurfaceType.Ground:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false);
                            break;
                        case SurfaceType.Water:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false, bonusRate: 0.5);
                            break;
                        case SurfaceType.Lava:
                            break;
                        case SurfaceType.Swamp:
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRaceType.Orc:
                    switch (_lastSurfaceType)
                    {
                        case SurfaceType.Ground:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false);
                            break;
                        case SurfaceType.Water:
                            break;
                        case SurfaceType.Lava:
                            break;
                        case SurfaceType.Swamp:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }


        public override string ToString()
        {
            //return string.Format("ID {0}, Name {1}, Race {2}, HP {3}, MP {4}, Str {5}, Agi {6}, Wis {7}, Con {8}  PAtk {9}, MAtk {10}",
            //    Id, Name, HumanoidRace, HP, MP, Strength, Agility, Wisdom, Constitution, PhysicalAttack, MagicalAttack);

            return string.Format("Id -> {0}\n" +
                                 "Race -> {1}\n" +
                                 "HP -> {2}\n" +
                                 "MP -> {3}\n" +
                                 "Strength -> {4}\n" +
                                 "Agility -> {5}\n" +
                                 "Wisdom -> {6}\n" +
                                 "Constitution -> {7}\n" +
                                 "Physical Attack -> {8}\n" +
                                 "Magical Attack -> {9}",
                                 Id, HumanoidRace, HP, MP, Strength, Agility, Wisdom, Constitution, PhysicalAttack, MagicalAttack);
        }
    }
}
