using Inveon.Services.OrderAPI.Dto;
using Inveon.Services.OrderAPI.Models;

namespace Inveon.Services.OrderAPI.Mapper
{
    public class OrderMapper
    {
        public static OrderHeaderDto MapToDto(OrderHeader orderHeader)
        {
            return new OrderHeaderDto
            {
                OrderHeaderId = orderHeader.OrderHeaderId,
                UserId = orderHeader.UserId,
                OrderTotal = orderHeader.OrderTotal,
                DiscountTotal = orderHeader.DiscountTotal,
                FirstName = orderHeader.FirstName,
                LastName = orderHeader.LastName,
                PickupDateTime = orderHeader.PickupDateTime,
                OrderTime = orderHeader.OrderTime,
                Phone = orderHeader.Phone,
                Email = orderHeader.Email,
                CardNumber = orderHeader.CardNumber,
                CartTotalItems = orderHeader.OrderDetails.Count, // Assuming OrderDetails is not null
                OrderDetails = MapToDto(orderHeader.OrderDetails)
            };
        }

        public static List<OrderDetailsDto> MapToDto(List<OrderDetails> orderDetailsList)
        {
            return orderDetailsList.Select(od => new OrderDetailsDto
            {
                OrderDetailsId = od.OrderDetailsId,
                ProductId = od.ProductId,
                Count = od.Count,
                ProductName = od.ProductName,
                Price = od.Price
            }).ToList();
        }
    }

}
