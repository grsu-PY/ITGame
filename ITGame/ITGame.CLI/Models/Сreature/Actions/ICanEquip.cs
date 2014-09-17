using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Creature.Actions
{
    public interface ICanEquip : IAction
    {
        void Equip(ITGame.CLI.Models.Equipment.Equipment equipment);
        void RemoveEquipment(Equipment.EquipmentType equipType);
    }
}
