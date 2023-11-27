using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Inveon.Services.FavoritesAPI.Model.Dto;

namespace Inveon.Services.FavoritesAPI.Model
{
    public class FavoriteItem
    {
        [Key]
        public int FavoriteItemID { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
    }
}
