using Elk.Core;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Pharmacy.API.Controllers
{
    [ApiController, CustomAuth, EnableCors("AllowedOrigins"), Route("[controller]")]
    public class OrderController : ControllerBase
    {
        readonly IUserService _userService;
        readonly IOrderService _orderService;
        readonly IPaymentService _paymentService;
        readonly IGatewayFactory _gatewayFectory;
        readonly IConfiguration _configuration;
        public OrderController(IUserService userService, IOrderService orderService, IPaymentService paymentService, IGatewayFactory gatewayFactory, IConfiguration configuration)
        {
            _userService = userService;
            _orderService = orderService;
            _paymentService = paymentService;
            _gatewayFectory = gatewayFactory;
            _configuration = configuration;
        }

        [HttpPost]

        public async Task<ActionResult<IResponse<AddOrderReponse>>> Add([FromServices] IOptions<CustomSetting> settings, OrderDTO model)
        {
            var addOrder = await _orderService.AddByEndUserAsync(User.GetUserId(), model);
            if (!addOrder.IsSuccessful) return new Response<AddOrderReponse> { Message = addOrder.Message };
            var fatcory = await _gatewayFectory.GetInsance(settings.Value.DefaultGatewayId);
            var transModel = new CreateTransactionRequest
            {
                OrderId = addOrder.Result.Order.OrderId,
                Amount = addOrder.Result.Order.TotalPrice,
                MobileNumber = User.Claims.First(x => x.Type == ClaimTypes.MobilePhone).Value,
                ApiKey = fatcory.Result.Gateway.MerchantId,
                CallbackUrl = fatcory.Result.Gateway.PostBackUrl,
                Url = fatcory.Result.Gateway.Url
            };
            var createTrans = await fatcory.Result.Service.CreateTrasaction(transModel, null);
            if (!createTrans.IsSuccessful) return new Response<AddOrderReponse> { Message = createTrans.Message, Result = new AddOrderReponse { OrderId = addOrder.Result.Order.OrderId } };
            var addPayment = await _paymentService.Add(transModel, createTrans.Result.TransactionId, fatcory.Result.Gateway.PaymentGatewayId);
            if (!addPayment.IsSuccessful) return new Response<AddOrderReponse> { Message = addPayment.Message, Result = new AddOrderReponse { OrderId = addOrder.Result.Order.OrderId } };
            return new Response<AddOrderReponse>
            {
                IsSuccessful = addPayment.IsSuccessful,
                Message = addPayment.Message,
                Result = new AddOrderReponse
                {
                    OrderId = addOrder.Result.Order.OrderId,
                    Url = createTrans.Result.GatewayUrl,
                    BasketChanged = addOrder.Result.IsChanged,
                    Drugs = addOrder.Result.Order.OrderItems.Select(x => new DrugDTO
                    {
                        DrugId = x.DrugId,
                        DiscountPrice = x.DiscountPrice,
                        Price = x.Price,
                        Count = x.Count
                    })
                }
            };
        }

    }
}