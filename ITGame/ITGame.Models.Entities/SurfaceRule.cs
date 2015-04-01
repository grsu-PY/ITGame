using System;
using ITGame.Models.Environment;
using ITGame.Models.Ñreature;

namespace ITGame.Models.Entities
{

    public class SurfaceRule : Identity
    {
        public SurfaceType CurrentSurfaceType { get; set; }

        public HumanoidRaceType HumanoidRaceType { get; set; }

        public int HP { get; set; }

        public int MP { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Wisdom { get; set; }

        public int Constitution { get; set; }

        public virtual Surface Surface { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
