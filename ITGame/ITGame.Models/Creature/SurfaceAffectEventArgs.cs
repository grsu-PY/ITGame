using ITGame.Models.Environment;
using System;

namespace ITGame.Models.Сreature
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
