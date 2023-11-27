using Inveon.Services.FavoritesAPI.Model.Dto;

namespace Inveon.Services.FavoritesAPI.Models.Dto
{
    public class FavoriteItemResponseDto
    {
        public int FavoriteItemID { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}
