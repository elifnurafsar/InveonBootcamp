using Inveon.Services.FavoritesAPI.Model.Dto;
using Inveon.Services.FavoritesAPI.Models.Dto;

namespace Inveon.Services.FavoritesAPI.Repository
{
    public interface IFavoriteItemRepository
    {
        Task<IEnumerable<FavoriteItemResponseDto>> GetFavoritesByUserId(string userId);
        Task<FavoriteItemDto> AddToFavorites(FavoriteItemDto favoriteItemDto);
        Task<bool> RemoveFromFavorites(FavoriteItemDto favoriteItemDto);
    }
}
