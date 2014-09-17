using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.CLI.Models.Environment
{
    public static class Surface
    {
        private static IDictionary<SurfaceType, SurfaceRule> _surfaceRules;

        static Surface()
        {
            _surfaceRules = new Dictionary<SurfaceType, SurfaceRule>();

            _surfaceRules.Add(SurfaceType.Ground, new SurfaceRule());
            _surfaceRules.Add(SurfaceType.Lava, new SurfaceRule { HP = -10, MP = -10, Strength = -5, Wisdom = -5, Agility = -5 });
            _surfaceRules.Add(SurfaceType.Swamp, new SurfaceRule { MP = -5, Wisdom = -1 });
            _surfaceRules.Add(SurfaceType.Water, new SurfaceRule { MP = 25, Wisdom = 5 });
        }

        public static void ConfigureRules(IDictionary<SurfaceType, SurfaceRule> surfaceRules)
        {
            foreach (var newRule in surfaceRules)
            {
                if (_surfaceRules.ContainsKey(newRule.Key))
                {
                    _surfaceRules[newRule.Key] = newRule.Value;
                }
                else
                {
                    _surfaceRules.Add(newRule);
                }
            }
        }

        public static void ApplyInfluence<T>(IEnumerable<T> creatures) where T : Creature.Creature
        {
            foreach (var creature in creatures)
            {
                creature.Name = "10";
            }
        }

        public static IDictionary<SurfaceType, SurfaceRule> CurrentRules
        {
            get
            {
                return _surfaceRules;
            }
        }
    }
}
