using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Creature.Actions
{
    public interface ICanTake
    {
        void Take(ITGame.CLI.Models.Items.Item item);
        void Drop(ITGame.CLI.Models.Items.Item item);
    }
}
