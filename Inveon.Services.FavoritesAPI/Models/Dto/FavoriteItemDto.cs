using System.ComponentModel.DataAnnotations.Schema;

namespace Inveon.Services.FavoritesAPI.Model.Dto
{
    public class FavoriteItemDto
    {
        public int FavoriteItemID { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
    }
}
