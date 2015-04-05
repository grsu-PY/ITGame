using System;
using System.Runtime.Serialization;
using ITGame.Infrastructure.Data;
using ITGame.Models.Environment;
using ITGame.Models.Ñreature;

namespace ITGame.Models.Entities
{
    [DataContract]
    public class Surface : Identity, IViewModelItem
    {
        [DataMember]
        public SurfaceType CurrentSurfaceType { get; set; }

        [DataMember]
        public HumanoidRaceType HumanoidRaceType { get; set; }

        [IgnoreDataMember]
        public SurfaceRule SurfaceRule { get; set; }

        [DataMember, ForeignKey(typeof(SurfaceRule))]
        public Guid SurfaceRuleId { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        #region IViewModelItem implementation

        [IgnoreDataMember]
        public bool IsSelectedModelItem { get; set; }

        #endregion
    }
}
