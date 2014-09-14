using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Equipment : ITGame.CLI.Models.Items.Item
    {
        protected EquipmentType equipmentType;

        public EquipmentType EquipmentType { get { return equipmentType; } }

    }
}
