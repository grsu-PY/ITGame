using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Helmet : Armor
    {
        public Helmet()
        {
            equipmentType = EquipmentType.Helmet;
        }
    }
}
