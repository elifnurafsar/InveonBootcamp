using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iyzipay.Request;
using Iyzipay.Model;
using Iyzipay;
using Inveon.Services.ShoppingCartAPI.Messages;
using Inveon.Services.ShoppingCartAPI.RabbitMQSender;
using Inveon.Services.ShoppingCartAPI.Repository;
using Inveon.Services.ShoppingCartAPI.Models.Dto;

namespace Inveon.Service.ShoppingCartAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/cartc")]
    public class CartAPICheckOutController : ControllerBase
    {

        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        // private readonly IMessageBus _messageBus;
        protected ResponseDto _response;
        private readonly IRabbitMQCartMessageSender _rabbitMQCartMessageSender;
        // IMessageBus messageBus,
        public CartAPICheckOutController(ICartRepository cartRepository,
            ICouponRepository couponRepository, IRabbitMQCartMessageSender rabbitMQCartMessageSender)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _rabbitMQCartMessageSender = rabbitMQCartMessageSender;
            //_messageBus = messageBus;
            this._response = new ResponseDto();
        }

        [HttpPost]
        [Authorize]
        public async Task<object> Checkout([FromBody] CheckoutHeaderDto checkoutHeaderDto)
        {
            try
            {
                
                CartDto cartDto = await _cartRepository.GetCartByUserId(checkoutHeaderDto.UserId);
                if (cartDto == null)
                {
                    return BadRequest();
                }
                CouponDto coupon = new CouponDto();
                coupon.DiscountAmount = 0;
                if (!string.IsNullOrEmpty(checkoutHeaderDto.CouponCode))
                {
                    coupon = await _couponRepository.GetCoupon(checkoutHeaderDto.CouponCode);
                    if (checkoutHeaderDto.DiscountTotal != coupon.DiscountAmount)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string>() { "Coupon Price has changed, please confirm" };
                        _response.DisplayMessage = "Coupon Price has changed, please confirm";
                        return _response;
                    }
                }

                checkoutHeaderDto.CartDetails = cartDto.CartDetails;
                //logic to add message to process order.
                // await _messageBus.PublishMessage(checkoutHeader, "checkoutqueue");

                //RabbitMQConsumer
                //Payment payment = OdemeIslemi(checkoutHeaderDto, coupon);
                checkoutHeaderDto.FirstName = "John";
                checkoutHeaderDto.LastName = "Doe";
                checkoutHeaderDto.Phone = "+905350000000";
                checkoutHeaderDto.Email = "email@email.com";
                checkoutHeaderDto.PickupDateTime = DateTime.Now;
                checkoutHeaderDto.CVV = "123";
                checkoutHeaderDto.ExpiryMonth = "12";
                checkoutHeaderDto.ExpiryYear = "2030";
                checkoutHeaderDto.CardNumber = "5528790000000008";
                _rabbitMQCartMessageSender.SendMessage(checkoutHeaderDto, "checkoutqueue");
                await _cartRepository.ClearCart(checkoutHeaderDto.UserId);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        public Payment OdemeIslemi(CheckoutHeaderDto checkoutHeaderDto, CouponDto coupon)
        {

            // iyzipay account credentials
            Options options = new Options();
            options.ApiKey = "sandbox-lfeCW8qmBvkEzMZ4NyWsY1wJv1ZdjR8P";
            options.SecretKey = "sandbox-2A3Zh3d8czLaxKnkY2f5ZdosFdq05deg";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            //request.Price = checkoutHeaderDto.OrderTotal.ToString().Replace(',', '.');
            //request.PaidPrice = checkoutHeaderDto.OrderTotal.ToString().Replace(',', '.');
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = "John Doe";
            paymentCard.CardNumber = "5528790000000008";
            paymentCard.ExpireMonth = "12";
            paymentCard.ExpireYear = "2030";
            paymentCard.Cvc = "123";
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            int sze = 0;
            foreach (CartDetailsDto detail in checkoutHeaderDto.CartDetails)
            {
                sze++;
            }

            double indirim = coupon.DiscountAmount * 1.0 / sze;
            double total = 0.0;
            List<BasketItem> basketItems = new List<BasketItem>();
            int i = 1;
            foreach (CartDetailsDto detail in checkoutHeaderDto.CartDetails)
            {
                BasketItem basketItem = new BasketItem();
                basketItem.Id = buyer.Id.ToString() + i.ToString();
                basketItem.Name = detail.Product.Name.ToString();
                basketItem.Category1 = detail.Product.CategoryName.ToString();
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                // Urunlerin fiyatinin indirim miktarindan daha pahali old varsayiliyor.
                double price = (detail.Product.Price * detail.Count - indirim);
                price = Math.Round(price, 2);
                if (price < 0)
                {
                    price = 0;
                }
                total+=price;
                basketItem.Price = price.ToString().Replace(',', '.');
                
                basketItems.Add(basketItem);
                i++;
            }
            request.BasketItems = basketItems;
            request.Price = total.ToString().Replace(',', '.');
            request.PaidPrice = total.ToString().Replace(',', '.');
            return Payment.Create(request, options);
        }
    }
}
