using System;
using ITGame.Models.Environment;
using ITGame.Models.Ñreature;

namespace ITGame.Models.Entities
{
    public class Surface : Identity
    {
        public SurfaceType CurrentSurfaceType { get; set; }

        public HumanoidRaceType HumanoidRaceType { get; set; }
        
        public virtual SurfaceRule SurfaceRule { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
