using System;

namespace ITGame.Models.Creature.Actions
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
