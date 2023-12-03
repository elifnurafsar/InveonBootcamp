using Inveon.Services.OrderAPI.Dto;
using Inveon.Services.OrderAPI.Models;
using Inveon.Services.OrderAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inveon.Services.OrderAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            this._response = new ResponseDto();
        }

        //dfaa01e1-b334-4e66-9c8c-d310f722188b
        //adf77f38-5a1e-4356-989d-78252db16ea0

        [HttpGet]
        [Authorize]
        [Route("{userId}")]

        public async Task<object> GetOrdersWithDetails(string userId)
        {
            try
            {
                IEnumerable<OrderHeaderDto> orders = await _orderRepository.GetOrdersWithDetails(userId);
                _response.Result = orders;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<object> GetAllOrdersWithDetails()
        {
            try
            {
                IEnumerable<OrderHeaderDto> orders = await _orderRepository.GetAllOrdersWithDetails();
                _response.Result = orders;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
