using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Armor : Equipment
    {
        public Armor()
        {
            equipmentType = EquipmentType.Armor;
        }
        public int PhysicalDef { get; set; }

        public int MagicalDef { get; set; }
    }
}
