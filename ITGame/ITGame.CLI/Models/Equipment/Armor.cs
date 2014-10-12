using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Armor : Equipment
    {
        private ArmorType armorType = ArmorType.None;

        public Armor()
        {
            equipmentType = EquipmentType.Armor;
        }

        [ParsingAttribute]
        public int PhysicalDef { get; set; }

        [ParsingAttribute]
        public int MagicalDef { get; set; }

        [ParsingAttribute]
        public ArmorType ArmorType
        {
            get { return armorType; }
            set { armorType = value; }
        }

        public override string ToString()
        {
            return string.Format("ID {0}, Name {1}, ArmorType {2}, PDef {3}, MDef{4}",
                Id, Name, ArmorType, PhysicalDef, MagicalDef);
        }
    }
}
