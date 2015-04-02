using System;
using System.Runtime.Serialization;
using ITGame.Models.Ñreature;

namespace ITGame.Models.Entities
{
    [DataContract]
    public class HumanoidRace : Identity
    {
        [DataMember]
        public HumanoidRaceType HumanoidRaceType { get; set; }

        [DataMember]
        public int Strength { get; set; }

        [DataMember]
        public int Agility { get; set; }

        [DataMember]
        public int Wisdom { get; set; }

        [DataMember]
        public int Constitution { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Guid Id { get; set; }
    }
}
