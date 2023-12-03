using Inveon.Services.OrderAPI.DbContexts;
using Inveon.Services.OrderAPI.Dto;
using Inveon.Services.OrderAPI.Mapper;
using Inveon.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Inveon.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;

        public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            if (orderHeader.CouponCode == null)
            {
                orderHeader.CouponCode = "";
            }
            await using var _db = new ApplicationDbContext(_dbContext);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderHeaderDto>> GetOrdersWithDetails(string userId)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            var orders = _db.OrderHeaders
               .Include(o => o.OrderDetails)
               .Where(o => o.UserId == userId)
               .ToList();

            var orderDtos = orders.Select(OrderMapper.MapToDto).ToList();
            return orderDtos;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus = paid;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderHeaderDto>> GetAllOrdersWithDetails()
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            var orders = _db.OrderHeaders
               .Include(o => o.OrderDetails)
               .ToList();

            var orderDtos = orders.Select(OrderMapper.MapToDto).ToList();
            return orderDtos;
        }

    }
}
