using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Equipment
    {
        protected EquipmentType equipmentType;

        public EquipmentType EquipmentType { get { return equipmentType; } }

        public int Name
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Weight
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
