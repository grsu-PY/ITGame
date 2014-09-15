using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.CLI.Models.Magic
{
    public class DefensiveSpell : Spell
    {
        private int totalDuration;
        private int currentDuration;

        public int TotalDuration
        {
            get { return totalDuration; }
            set { totalDuration = value; }
        }
        public int Duration
        {
            get { return currentDuration; }
            set { currentDuration = value; }
        }
    }
}
