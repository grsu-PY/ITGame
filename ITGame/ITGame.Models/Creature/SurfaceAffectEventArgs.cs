using System;
using ITGame.Models.Environment;

namespace ITGame.Models.Creature
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
