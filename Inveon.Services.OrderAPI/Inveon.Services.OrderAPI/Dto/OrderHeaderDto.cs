using Inveon.Services.OrderAPI.Models;

namespace Inveon.Services.OrderAPI.Dto
{
    public class OrderHeaderDto
    {
        public int OrderHeaderId { get; set; }
        public string UserId { get; set; }
        public double OrderTotal { get; set; }
        public double DiscountTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime PickupDateTime { get; set; }
        public DateTime OrderTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public int CartTotalItems { get; set; }
        public List<OrderDetailsDto> OrderDetails { get; set; }
    }
}
