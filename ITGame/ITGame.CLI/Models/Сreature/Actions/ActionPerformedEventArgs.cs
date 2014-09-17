using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Creature.Actions
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
