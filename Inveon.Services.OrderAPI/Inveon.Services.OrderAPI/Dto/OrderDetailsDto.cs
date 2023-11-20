using System.ComponentModel.DataAnnotations.Schema;

namespace Inveon.Services.OrderAPI.Dto
{
    public class OrderDetailsDto
    {
        public int OrderDetailsId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
