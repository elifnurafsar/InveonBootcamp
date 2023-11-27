using Inveon.Services.FavoritesAPI.Model.Dto;
using Newtonsoft.Json;

namespace Inveon.Services.FavoritesAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient client;

        public ProductRepository(HttpClient client)
        {
            this.client = client;
        }

        public async Task<ProductDto> GetProduct(int id)
        {
            var response = await client.GetAsync($"/api/products/{id}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp != null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(resp.Result));
            }
            return new ProductDto();
        }
    }
}
