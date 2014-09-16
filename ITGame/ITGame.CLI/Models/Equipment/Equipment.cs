using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Equipment : ITGame.CLI.Models.Items.Item
    {
        protected EquipmentType equipmentType = EquipmentType.None;
        protected WeaponType weaponType = WeaponType.None;
        protected ArmorType armorType = ArmorType.None;

        public EquipmentType EquipmentType
        {
            get { return equipmentType; }
            set { equipmentType = value; }
        }

        public WeaponType WeaponType
        {
            get { return weaponType; }
            set { weaponType = value; }
        }

        public ArmorType ArmorType
        {
            get { return armorType; }
            set { armorType = value; }
        }
    }
}
