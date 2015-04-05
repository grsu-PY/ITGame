using System;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Ñreature;

namespace ITGame.Models.Entities
{
    [DataContract]
    public class HumanoidRace : Identity, IViewModelItem
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

        #region IViewModelItem implementation

        [IgnoreDataMember]
        public bool IsSelectedModelItem { get; set; }

        #endregion
    }
}
