using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Weapon : Equipment
    {
        private int itemId;
        private int physicalAttack;
        private int magicalAttack;
        private bool isAttack;

        public Weapon()
        {
            equipmentType = EquipmentType.Weapon;
        }

        public int ItemID
        {
            get { return itemId; }
            set { itemId = value; }
        }

        public bool IsAttack
        {
            get { return isAttack; }
            set { isAttack = value; }
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
                return physicalAttack;
            }
            set
            {
                physicalAttack = value;
            }
        }

        public int MagicalAttack
        {
            get
            {
                return magicalAttack;
            }
            set
            {
                magicalAttack = value;
            }
        }
    }
}
