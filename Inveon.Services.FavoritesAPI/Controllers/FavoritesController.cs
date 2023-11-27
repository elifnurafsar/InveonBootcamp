using Inveon.Services.FavoritesAPI.Model.Dto;
using Inveon.Services.FavoritesAPI.Models.Dto;
using Inveon.Services.FavoritesAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inveon.Services.FavoritesAPI.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    //[EnableCors("AllowSpecificOrigin")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteItemRepository favoriteItemRepository;
        protected ResponseDto _response;

        public FavoritesController(IFavoriteItemRepository _favoriteItemRepository )
        {
            favoriteItemRepository = _favoriteItemRepository;
            this._response = new ResponseDto();
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<object> GetFavorites(string userId)
        {
            try
            {
                IEnumerable<FavoriteItemResponseDto> favoriteItems = await favoriteItemRepository.GetFavoritesByUserId(userId);

                if (favoriteItems == null)
                {
                    _response.IsSuccess = true;
                    _response.ErrorMessages = new List<string>() { "The favorites list is empty" };
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Result = favoriteItems;

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return _response;
            }
        }

        [HttpPost]
        //[Authorize]
        public async Task<object> Post([FromBody] FavoriteItemDto favoriteItemDto)
        {
            try
            {
                FavoriteItemDto model = await favoriteItemRepository.AddToFavorites(favoriteItemDto);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete]
        //[Authorize]
        public async Task<object> Delete([FromBody] FavoriteItemDto favoriteItemDto)
        {
            try
            {
                bool isSuccess = await favoriteItemRepository.RemoveFromFavorites(favoriteItemDto);
                _response.Result = isSuccess;
                if(!isSuccess)
                {
                    _response.DisplayMessage = "The service could not find the item: " + favoriteItemDto.ProductId + " or could not complete the process.";
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpOptions]
        public IActionResult Options()
        {
            return Ok();
        }
    }
}
