using Inveon.Services.FavoritesAPI.Model.Dto;

namespace Inveon.Services.FavoritesAPI.Repository
{
    public interface IProductRepository
    {
        Task<ProductDto> GetProduct(int id);
    }
}
