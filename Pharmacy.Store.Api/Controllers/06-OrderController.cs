using Elk.Core;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
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
        readonly IOrderService _orderService;
        readonly IGatewayFactory _gatewayFectory;
        readonly APICustomSetting _setting;
        public OrderController(IOrderService orderService,
            IGatewayFactory gatewayFactory,
            IOptions<APICustomSetting> settings)
        {
            _orderService = orderService;
            _gatewayFectory = gatewayFactory;
            _setting = settings.Value;
        }

        [HttpPost]

        public async Task<ActionResult<IResponse<AddOrderReponse>>> Add(OrderDTO model)
        {
            var addOrder = await _orderService.AddByEndUser(User.GetUserId(), model);
            if (!addOrder.IsSuccessful) return new Response<AddOrderReponse>
            {
                Message = addOrder.Message,
                Result = addOrder.Result.IsChanged ? new AddOrderReponse
                {
                    BasketChanged = addOrder.Result.IsChanged,
                    Drugs = addOrder.Result.Order.OrderItems.Select(x => new DrugDTO
                    {
                        DrugId = x.DrugId,
                        DiscountPrice = x.DiscountPrice,
                        Price = x.Price,
                        Count = x.Count
                    })
                } : new AddOrderReponse { BasketChanged = false }
            };
            var fatcory = await _gatewayFectory.GetInsance(_setting.DefaultGatewayId);
            var transModel = new CreateTransactionRequest
            {
                OrderId = addOrder.Result.Order.OrderId,
                PaymentType = PaymentType.Order,
                GatewayId = _setting.DefaultGatewayId,
                Amount = addOrder.Result.Order.TotalPrice,
                MobileNumber = User.Claims.First(x => x.Type == ClaimTypes.MobilePhone).Value,
                ApiKey = fatcory.Result.Gateway.MerchantId,
                CallbackUrl = fatcory.Result.Gateway.PostBackUrl,
                Url = fatcory.Result.Gateway.Url
            };
            var createTrans = await fatcory.Result.Service.CreateTransaction(transModel, null);
            if (!createTrans.IsSuccessful) return new Response<AddOrderReponse> { Message = createTrans.Message };
            return new Response<AddOrderReponse>
            {
                IsSuccessful = createTrans.IsSuccessful,
                Message = createTrans.Message,
                Result = new AddOrderReponse
                {
                    OrderId = addOrder.Result.Order.OrderId,
                    Url = createTrans.Result.GatewayUrl
                }
            };
        }


    }
}