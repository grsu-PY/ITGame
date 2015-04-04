
using ITGame.Infrastructure.Data;

namespace ITGame.Models.Equipment
{
    public class Armor : Equipment
    {
        private ArmorType armorType = ArmorType.None;

        public Armor()
        {
            equipmentType = EquipmentType.Armor;
        }

        [Column]
        public int PhysicalDef { get; set; }

        [Column]
        public int MagicalDef { get; set; }

        [Column]
        public ArmorType ArmorType
        {
            get { return armorType; }
            set { armorType = value; }
        }

        public override string ToString()
        {
            //return string.Format("ID {0}, Name {1}, ArmorType {2}, PDef {3}, MDef{4}",
            //    Id, Name, ArmorType, PhysicalDef, MagicalDef);

            return string.Format("Id -> {0}\n"+
                                 "Armor Type -> {1}\n"+
                                 "Physical Defense -> {2}\n"+
                                 "Magical Defense -> {3}",
                                 Id, ArmorType, PhysicalDef, MagicalDef);
        }
    }
}
