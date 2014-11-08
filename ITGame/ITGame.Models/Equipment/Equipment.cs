
namespace ITGame.Models.Equipment
{
    public class Equipment : ITGame.Models.Items.Item
    {
        protected EquipmentType equipmentType = EquipmentType.None;

        public EquipmentType EquipmentType
        {
            get { return equipmentType; }
            set { equipmentType = value; }
        }

    }
}
