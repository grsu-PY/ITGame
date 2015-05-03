using System;
using System.Collections.Generic;
using ITGame.Infrastructure.Data;
using ITGame.Models.Сreature;

namespace ITGame.Models.Administration
{
    public class Character : Identity
    {
        public Character()
        {
            Humanoids = new List<Humanoid>();
        }

        public Guid Id { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public RoleType Role { get; set; }

        public List<Humanoid> Humanoids { get; set; }
    }
}
