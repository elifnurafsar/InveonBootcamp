using Inveon.Services.OrderAPI.Dto;
using Inveon.Services.OrderAPI.Models;

namespace Inveon.Services.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader orderHeader);
        Task<IEnumerable<OrderHeaderDto>> GetOrdersWithDetails(string userId);
        Task<IEnumerable<OrderHeaderDto>> GetAllOrdersWithDetails();
        Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid);
    }
}
