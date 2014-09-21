using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Weapon : Equipment
    {
        public Weapon()
        {
            equipmentType = EquipmentType.Weapon;
        }

        public bool IsAttack { get; set; }


        public int PhysicalAttack { get; set; }

        public int MagicalAttack { get; set; }
    }
}
