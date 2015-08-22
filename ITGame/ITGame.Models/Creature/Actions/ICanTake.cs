using System;

namespace ITGame.Models.Creature.Actions
{
    public interface ICanTake : IAction
    {
        void Take(Items.Item item);
        void Drop(Guid itemId);
    }
}
