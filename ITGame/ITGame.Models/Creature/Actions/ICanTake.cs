using System;

namespace ITGame.Models.Сreature.Actions
{
    public interface ICanTake : IAction
    {
        void Take(Items.Item item);
        void Drop(Guid itemId);
    }
}
