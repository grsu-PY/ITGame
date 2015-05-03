using System;
using System.Linq;
using AutoMapper;

namespace ITGame.Models.Entities.Mapping
{
    public static class EntitiesMapper
    {
        private static bool RegisterMappings()
        {
            Mapper.CreateMap<Models.Сreature.Humanoid, Models.Entities.Humanoid>()
                .ForMember(humanoid => humanoid.HumanoidRaceType,
                    expression => expression.MapFrom(humanoid => humanoid.HumanoidRace))
                .ForMember(humanoid => humanoid.WeaponIds,
                    expression => expression.MapFrom(humanoid => humanoid.Weapons.Select(x => x.Id).ToList()))
                .ForMember(humanoid => humanoid.SpellIds,
                    expression => expression.MapFrom(humanoid => humanoid.Spells.Select(x => x.Id).ToList()))
                .ForMember(humanoid => humanoid.ArmorIds,
                    expression => expression.MapFrom(humanoid => humanoid.Armors.Select(x => x.Id).ToList()));

            Mapper.CreateMap<Models.Administration.Character, Models.Entities.Character>()
                .ForMember(character => character.HumanoidIds,
                    expression => expression.MapFrom(character => character.Humanoids.Select(x => x.Id).ToList()));

            Mapper.CreateMap<Models.Equipment.Weapon, Models.Entities.Weapon>();
            Mapper.CreateMap<Models.Equipment.Armor, Models.Entities.Armor>();
            Mapper.CreateMap<Models.Magic.Spell, Models.Entities.Spell>();
            Mapper.CreateMap<Models.Environment.Surface, Models.Entities.Surface>();
            Mapper.CreateMap<Models.Environment.SurfaceRule, Models.Entities.SurfaceRule>();
            

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
