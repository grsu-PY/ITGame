﻿using ITGame.CLI.Models.Creature.Actions;
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
        protected HumanoidRace humanoidRace;

        protected Dictionary<Guid, Item> items;
        protected Dictionary<string, Weapon> weapons;
        protected Dictionary<string, Armor> armors;
        protected Dictionary<string, Spell> spells;

        protected Armor body;
        protected Armor boots;
        protected Armor gloves;
        protected Armor helmet;

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
                case EquipmentType.Armor:
                    var armor = equipment as Armor;
                    switch (armor.ArmorType)
                    {
                        case ArmorType.Body:
                            body = armor;
                            pDef += body.PhysicalDef;
                            mDef += body.MagicalDef;
                            break;
                        case ArmorType.Boots:
                            boots = armor;
                            pDef += boots.PhysicalDef;
                            mDef += boots.MagicalDef;
                            break;
                        case ArmorType.Gloves:
                            gloves = armor;
                            pDef += gloves.PhysicalDef;
                            mDef += gloves.MagicalDef;
                            break;
                        case ArmorType.Helmet:
                            helmet = armor;
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
                            body = null;
                            pDef -= body.PhysicalDef;
                            mDef -= body.MagicalDef;
                            break;
                        case ArmorType.Boots:
                            boots = null;
                            pDef -= boots.PhysicalDef;
                            mDef -= boots.MagicalDef;
                            break;
                        case ArmorType.Gloves:
                            gloves = null;
                            pDef -= gloves.PhysicalDef;
                            mDef -= gloves.MagicalDef;
                            break;
                        case ArmorType.Helmet:
                            helmet = null;
                            pDef -= helmet.PhysicalDef;
                            mDef -= helmet.MagicalDef;
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

        public void SelectSpell(AttackSpell selectedAttackSpell = null, DefensiveSpell selectedDefensiveSpell = null)
        {
            this.attackSpell = selectedAttackSpell;
            this.defensiveSpell = selectedDefensiveSpell;
        }

        public HumanoidRace HumanoidRace
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
                return base.PhysicalAttack + ((weapon != null && weapon.IsAttack != false) ? weapon.PhysicalAttack : 0);
            }
        }

        public override int MagicalAttack
        {
            get
            {
                return base.MagicalAttack + ((weapon != null && weapon.IsAttack != false) ? weapon.MagicalAttack :
                                                             ((attackSpell != null && attackSpell.IsAttack != false) ? attackSpell.MagicalPower : 0));
            }
        }

        public override void RecieveDamage(Damage damage, SpellType spellType = SpellType.None)
        {
            var mDef = MagicalDefence;

            if (spellType != SpellType.None)
                mDef += (defensiveSpell != null &&
                        (defensiveSpell.Duration != 0 && defensiveSpell.SpellType == spellType)) ? defensiveSpell.MagicalPower : 0;

            if (defensiveSpell != null)
                defensiveSpell.Duration -= defensiveSpell.Duration != 0 ? 1 : 0;

            damage.PhysicalDamage -= pDef;
            damage.MagicalDamage -= mDef;

            base.RecieveDamage(damage);
        }

        public override void WeaponAttack()
        {
            if (_target == null || weapon == null) return;

            weapon.IsAttack = true;

            var message = string.Format("Your potential damage is {0}", PhysicalAttack);

            OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Fight));

            _target.RecieveDamage(new Damage
            {
                PhysicalDamage = PhysicalAttack,
                MagicalDamage = MagicalAttack
            });

            weapon.IsAttack = false;
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

            attackSpell.IsAttack = true;
            message = string.Format("Your potential damage is {0}", MagicalAttack);

            OnActionPerformed(new ActionPerformedEventArgs(message, ActionType.Fight));

            _target.RecieveDamage(new Damage
            {
                PhysicalDamage = 0,
                MagicalDamage = MagicalAttack
            }, attackSpell.SpellType);

            ManaConsumption(attackSpell.ManaCost);
            attackSpell.IsAttack = false;
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

            defensiveSpell.Duration = defensiveSpell.TotalDuration;
            ManaConsumption(defensiveSpell.ManaCost);
        }

        public Weapon Weapon { get { return weapon; } }
        public AttackSpell AttackSpell { get { return attackSpell; } }
        public DefensiveSpell DefensiveSpell { get { return defensiveSpell; } }

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
                case HumanoidRace.Dwarf:
                    switch (e.surfaceType)
                    {
                        case ITGame.CLI.Models.Environment.SurfaceType.Ground:
                            ApplySurfaceRule(e.actionRule, bonus: true, bonusRate: 1.2);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Water:
                            ApplySurfaceRule(e.actionRule, bonus: true, bonusRate: 0.2);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Lava:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 0.8);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Swamp:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 0.6);
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRace.Elf:
                    switch (e.surfaceType)
                    {
                        case ITGame.CLI.Models.Environment.SurfaceType.Ground:
                            ApplySurfaceRule(e.actionRule, bonus: true);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Water:
                            ApplySurfaceRule(e.actionRule, bonus: true);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Lava:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 1.5);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Swamp:
                            ApplySurfaceRule(e.actionRule, bonus: false);
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRace.Human:
                    switch (e.surfaceType)
                    {
                        case ITGame.CLI.Models.Environment.SurfaceType.Ground:
                            ApplySurfaceRule(e.actionRule, bonus: true);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Water:
                            ApplySurfaceRule(e.actionRule, bonus: true, bonusRate: 0.5);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Lava:
                            ApplySurfaceRule(e.actionRule, bonus: false);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Swamp:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 0.5);
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRace.Orc:
                    switch (e.surfaceType)
                    {
                        case ITGame.CLI.Models.Environment.SurfaceType.Ground:
                            ApplySurfaceRule(e.actionRule, bonus: true);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Water:
                            ApplySurfaceRule(e.actionRule, bonus: false);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Lava:
                            ApplySurfaceRule(e.actionRule, bonus: false, bonusRate: 0.8);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Swamp:
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
                case HumanoidRace.Dwarf:
                    switch (_lastSurfaceType)
                    {
                        case ITGame.CLI.Models.Environment.SurfaceType.Ground:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false, bonusRate: 1.2);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Water:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false, bonusRate: 0.2);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Lava:
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Swamp:
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRace.Elf:
                    switch (_lastSurfaceType)
                    {
                        case ITGame.CLI.Models.Environment.SurfaceType.Ground:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Water:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Lava:
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Swamp:
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRace.Human:
                    switch (_lastSurfaceType)
                    {
                        case ITGame.CLI.Models.Environment.SurfaceType.Ground:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Water:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false, bonusRate: 0.5);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Lava:
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Swamp:
                            break;
                        default:
                            break;
                    }
                    break;
                case HumanoidRace.Orc:
                    switch (_lastSurfaceType)
                    {
                        case ITGame.CLI.Models.Environment.SurfaceType.Ground:
                            ApplySurfaceRule(_lastSurfaceRule, bonus: false);
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Water:
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Lava:
                            break;
                        case ITGame.CLI.Models.Environment.SurfaceType.Swamp:
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
        
    }
}
