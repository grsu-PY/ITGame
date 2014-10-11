using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Equipment
{
    public class Weapon : Equipment
    {
        private WeaponType weaponType = WeaponType.None;

        public Weapon()
        {
            equipmentType = EquipmentType.Weapon;
        }

        public int PhysicalAttack { get; set; }

        public int MagicalAttack { get; set; }

        public WeaponType WeaponType
        {
            get { return weaponType; }
            set { weaponType = value; }
        }

        public override string ToString()
        {
            return string.Format("ID {0}, Name {1}, WeaponType {2}, PAtk {3}, MAtk {4}",
                Id, Name, WeaponType, PhysicalAttack, MagicalAttack);
        }
    }
}
