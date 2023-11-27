using AutoMapper;
using Inveon.Services.FavoritesAPI.DbContexts;
using Inveon.Services.FavoritesAPI.Model;
using Inveon.Services.FavoritesAPI.Model.Dto;
using Inveon.Services.FavoritesAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Inveon.Services.FavoritesAPI.Repository
{
    public class FavoriteItemRepository : IFavoriteItemRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IProductRepository productRepository;
        private IMapper _mapper;

        //Constructor Injection 
        public FavoriteItemRepository(ApplicationDbContext db, IMapper mapper, IProductRepository _productRepository)
        {
            _db = db;
            _mapper = mapper;
            productRepository = _productRepository;
        }

        public async Task<FavoriteItemDto> AddToFavorites(FavoriteItemDto favoriteItemDto)
        {
            // Map FavoriteItemDto to FavoriteItem entity
            var favoriteItem = _mapper.Map<FavoriteItem>(favoriteItemDto);

            // Add to the database
            _db.FavoriteItems.Add(favoriteItem);
            await _db.SaveChangesAsync();

            // Map the added entity back to FavoriteItemDto
            var addedFavoriteItemDto = _mapper.Map<FavoriteItemDto>(favoriteItem);

            return addedFavoriteItemDto;
        }

        public async Task<IEnumerable<FavoriteItemResponseDto>> GetFavoritesByUserId(string userId)
        {
            try
            {
                var favoriteItems = await _db.FavoriteItems
                    .Where(fi => fi.UserId == userId)
                    .ToListAsync();

                var favoriteItemDtos = new List<FavoriteItemResponseDto>();

                foreach (var favoriteItem in favoriteItems)
                {
                    var favoriteItemDto = new FavoriteItemResponseDto
                    {
                        FavoriteItemID = favoriteItem.FavoriteItemID,
                        UserId = favoriteItem.UserId,
                        ProductId = favoriteItem.ProductId
                    };

                    // Fetch the product information using the productRepository
                    var productDto = await productRepository.GetProduct(favoriteItem.ProductId);
                    favoriteItemDto.ProductDto = productDto ?? new ProductDto();

                    favoriteItemDtos.Add(favoriteItemDto);
                }

                return favoriteItemDtos;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                throw;
            }
        }

        public async Task<bool> RemoveFromFavorites(FavoriteItemDto favoriteItemDto)
        {
            try
            {
                var favoriteItem = await _db.FavoriteItems
                    .FirstOrDefaultAsync(f => f.FavoriteItemID == favoriteItemDto.ProductId && f.UserId == favoriteItemDto.UserId);

                if (favoriteItem != null)
                {
                    _db.FavoriteItems.Remove(favoriteItem);
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false; // Favorite item not found
            }
            catch (Exception ex)
            {
                // Handle exception (log, throw, etc.)
                return false;
            }
        }
    }
}
