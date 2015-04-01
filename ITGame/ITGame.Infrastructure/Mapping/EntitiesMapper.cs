using System;
using System.Linq;
using AutoMapper;

namespace ITGame.Infrastructure.Mapping
{
    public static class EntitiesMapper
    {
        private static bool RegisterMappings()
        {
            Mapper.CreateMap<Models.Сreature.Humanoid, Models.Entities.Humanoid>()
                .ForMember(humanoid => humanoid.HumanoidRaceType,
                    expression => expression.MapFrom(humanoid => humanoid.HumanoidRace))
                .ForMember(humanoid => humanoid.Weapons,
                    expression => expression.MapFrom(humanoid => humanoid.Weapons.Select(x => x.Id).ToList()))
                .ForMember(humanoid => humanoid.Spells,
                    expression => expression.MapFrom(humanoid => humanoid.Spells.Select(x => x.Id).ToList()))
                .ForMember(humanoid => humanoid.Armors,
                    expression => expression.MapFrom(humanoid => humanoid.Armors.Select(x => x.Id).ToList()));

            return true;
        }

        public static TDestination Map<TDestination>(this object source)
        {
            if (Mapper.FindTypeMapFor(source.GetType(), typeof(TDestination)) == null)
            {
                Mapper.CreateMap(source.GetType(), typeof(TDestination));
            }

            return Mapper.Map<TDestination>(source);
        }

        private static bool _mapCreated = RegisterMappings();
    }
}
