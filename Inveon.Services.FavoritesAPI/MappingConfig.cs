using AutoMapper;
using Inveon.Services.FavoritesAPI.Model.Dto;
using Inveon.Services.FavoritesAPI.Model;
using Inveon.Services.FavoritesAPI.Models.Dto;

namespace Inveon.Services.FavoritesAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<FavoriteItemDto, FavoriteItem>()
                    .ForMember(dest => dest.FavoriteItemID, opt => opt.Ignore())
                    .ReverseMap();
            });

            return mappingConfig;
        }
    }
}
