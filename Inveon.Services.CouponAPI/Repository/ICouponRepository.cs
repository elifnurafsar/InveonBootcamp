using Inveon.Services.CouponAPI.Models.Dto;

namespace Inveon.Services.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string couponCode);
    }
}
