using ITGame.CLI.Models.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Creature
{
    public class SurfaceAffectEventArgs : EventArgs
    {
        public readonly SurfaceRule actionRule;
        public readonly SurfaceType surfaceType;

        public SurfaceAffectEventArgs(SurfaceType surfaceType, SurfaceRule actionRule)
        {
            this.actionRule = actionRule;
            this.surfaceType = surfaceType;
        }
    }
}
