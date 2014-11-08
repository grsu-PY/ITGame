
namespace ITGame.Models.Equipment
{
    public class Weapon : Equipment
    {
        private WeaponType weaponType = WeaponType.None;

        public Weapon()
        {
            equipmentType = EquipmentType.Weapon;
        }

        [Column]
        public int PhysicalAttack { get; set; }

        [Column]
        public int MagicalAttack { get; set; }

        [Column]
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
