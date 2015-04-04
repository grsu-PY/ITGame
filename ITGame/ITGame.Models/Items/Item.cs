using System;
using ITGame.Infrastructure.Data;

namespace ITGame.Models.Items
{
    public abstract class Item : Identity
    {
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
