using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI
{
    public interface IAction
    {
        event EventHandler<ActionPerformedEventArgs> ActionPerformed;
    }
}
