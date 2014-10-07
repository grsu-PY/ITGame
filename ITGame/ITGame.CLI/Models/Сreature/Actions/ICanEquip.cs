namespace ITGame.CLI.Models.Сreature.Actions
{
    public interface ICanEquip : IAction
    {
        void Equip(Equipment.Equipment equipment);
        void RemoveEquipment(Equipment.Equipment equipment);
    }
}
