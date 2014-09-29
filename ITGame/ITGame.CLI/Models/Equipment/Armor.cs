using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Armor : Equipment
    {
        protected ArmorType armorType = ArmorType.None;

        public Armor()
        {
            equipmentType = EquipmentType.Armor;
        }
        public int PhysicalDef { get; set; }

        public int MagicalDef { get; set; }

        public ArmorType ArmorType
        {
            get { return armorType; }
            set { armorType = value; }
        }
    }
}
