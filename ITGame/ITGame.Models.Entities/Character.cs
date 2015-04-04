using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;

namespace ITGame.Models.Entities
{
    [DataContract]
    public class Character : Identity
    {
        public Character()
        {
            HumanoidIds = new HashSet<Guid>();

            Humanoids = new HashSet<Humanoid>();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public byte Role { get; set; }

        [DataMember]
        [ForeignKey(typeof(Humanoid), IsCollection = true)]
        public ICollection<Guid> HumanoidIds { get; set; }

        [IgnoreDataMember]
        public ICollection<Humanoid> Humanoids { get; set; }
    }

    
}
