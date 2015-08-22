using System;

namespace ITGame.Models.Creature.Actions
{
    public interface IAction
    {
        event EventHandler<ActionPerformedEventArgs> ActionPerformed;
    }
}
