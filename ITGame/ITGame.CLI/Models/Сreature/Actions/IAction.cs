using System;

namespace ITGame.CLI.Models.Сreature.Actions
{
    public interface IAction
    {
        event EventHandler<ActionPerformedEventArgs> ActionPerformed;
    }
}
