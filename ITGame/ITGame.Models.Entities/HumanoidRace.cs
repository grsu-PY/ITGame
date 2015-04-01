using System;
using ITGame.Models.�reature;

namespace ITGame.Models.Entities
{
    public class HumanoidRace : Identity
    {
        public HumanoidRaceType HumanoidRaceType { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Wisdom { get; set; }

        public int Constitution { get; set; }

        public string Name { get; set; }

        public Guid Id { get; set; }
    }
}
