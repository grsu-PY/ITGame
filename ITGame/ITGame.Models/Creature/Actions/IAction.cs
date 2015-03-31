using System;

namespace ITGame.Models.Сreature.Actions
{
    public interface IAction
    {
        event EventHandler<ActionPerformedEventArgs> ActionPerformed;
    }
}
