using System;
using System.Collections.Generic;

namespace ITGame.Models.Entities
{
    public class Character : Identity
    {
        public Character()
        {
            Humanoids = new HashSet<Guid>();
        }
        public Guid Id { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public byte Role { get; set; }

        public ICollection<Guid> Humanoids { get; set; }
    }
}
