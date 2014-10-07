using System;

namespace ITGame.CLI.Models.Сreature.Actions
{
    public interface ICanTake : IAction
    {
        void Take(Items.Item item);
        void Drop(Guid itemId);
    }
}
