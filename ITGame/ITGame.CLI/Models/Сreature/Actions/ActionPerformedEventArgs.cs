using System;

namespace ITGame.CLI.Models.Сreature.Actions
{
    public class ActionPerformedEventArgs:EventArgs
    {
        public readonly string message;
        public readonly ActionType actionType;

        public ActionPerformedEventArgs(string message, ActionType actionType)
        {
            this.message = message;
            this.actionType = actionType;
        }
    }
}
