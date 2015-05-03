using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITGame.Infrastructure.Data;
using ITGame.Models.Сreature;

namespace ITGame.Models.Creature
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
