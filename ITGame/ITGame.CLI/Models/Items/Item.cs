using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITGame.CLI.Models.Items
{
    public abstract class Item : ITGame.CLI.Models.Identity
    {
        public Guid Id { get; set; }

        [ParsingAttribute]
        public virtual string Name { get; set; }

        protected int weight = 1;


        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }


    }
}
