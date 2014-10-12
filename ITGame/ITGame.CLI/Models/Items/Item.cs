using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Items
{
    public abstract class Item : ITGame.CLI.Models.Identity
    {
        [Column]
        public Guid Id { get; set; }

        [Column]
        public virtual string Name { get; set; }

        protected int weight = 1;

        [Column]
        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }


    }
}
