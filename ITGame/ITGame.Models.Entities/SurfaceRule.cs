using System;
using System.Runtime.Serialization;
using ITGame.Models.Entities.Attributes;
using ITGame.Models.Environment;
using ITGame.Models.Ñreature;

namespace ITGame.Models.Entities
{

    [DataContract]
    public class SurfaceRule : Identity
    {
        [DataMember]
        public SurfaceType CurrentSurfaceType { get; set; }

        [DataMember]
        public HumanoidRaceType HumanoidRaceType { get; set; }

        [DataMember]
        public int HP { get; set; }

        [DataMember]
        public int MP { get; set; }

        [DataMember]
        public int Strength { get; set; }

        [DataMember]
        public int Agility { get; set; }

        [DataMember]
        public int Wisdom { get; set; }

        [DataMember]
        public int Constitution { get; set; }

        [IgnoreDataMember]
        public Surface Surface { get; set; }

        [DataMember,ForeignKey(typeof(Surface))]
        public SurfaceType SurfaceId { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
