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

        public bool IsAttack
        {
            get { throw new System.NotImplementedException(); }
            set { }
        }

        public int AttackSpeed
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int PhysicalAttack
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int MagicalAttack
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
