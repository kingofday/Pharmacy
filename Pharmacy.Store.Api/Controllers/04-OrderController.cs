using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Store.Api.Resources;
using Microsoft.Extensions.Configuration;

namespace Pharmacy.Store.Api.Controllers
{
    public class OrderController : Controller
    {
        readonly IUserService _userService;
        readonly IOrderService _orderService;
        readonly IDeliveryService _deliveryService;
        readonly IPaymentService _paymentService;
        readonly IGatewayFactory _gatewayFectory;
        readonly IConfiguration _configuration;
        public OrderController(IUserService userService, IOrderService orderService, IDeliveryService deliveryService, IPaymentService paymentService, IGatewayFactory gatewayFactory, IConfiguration configuration)
        {
            _userService = userService;
            _orderService = orderService;
            _paymentService = paymentService;
            _gatewayFectory = gatewayFactory;
            _deliveryService = deliveryService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CompleteInfo([FromBody]UserDTO model)
        {
            if (model.Token != null)
            {
                var findUser = await _userService.FindAsync(model.Token ?? Guid.Empty);
                if (findUser.IsSuccessful)
                    return Json(new
                    {
                        IsSuccessful = true,
                        Result = findUser.Result.UserId
                    });
            }

            var prevUser = await _userService.FindByMobileNumber(model.MobileNumber);
            if (prevUser.IsSuccessful) return Json(new
            {
                IsSuccessful = true,
                Result = prevUser.Result.UserId
            });
            var addUser = await _userService.AddAsync(new User
            {
                MobileNumber = model.MobileNumber,
                FullName = model.Fullname,
                Password = HashGenerator.Hash(model.MobileNumber.ToString())
            });
            return Json(new
            {
                addUser.IsSuccessful,
                addUser.Message,
                Result = addUser.IsSuccessful ? addUser.Result.UserId : Guid.Empty
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody]OrderDTO order)
        {
            var findUser = await _userService.FindAsync(order.UserToken);
            if (!findUser.IsSuccessful) return Json(new Response<string> { Message = Strings.InvalidToken });
            var addOrder = await _orderService.Add(order);
            if (!addOrder.IsSuccessful) return Json(new Response<AddOrderReponse> { Message = addOrder.Message });
            var fatcory = await _gatewayFectory.GetInsance(int.Parse(_configuration["DefaultGatewayId"]));
            var transModel = new CreateTransactionRequest
            {
                OrderId = addOrder.Result.Order.OrderId,
                Amount = addOrder.Result.Order.TotalPriceAfterDiscount,
                MobileNumber = findUser.Result.MobileNumber.ToString(),
                ApiKey = fatcory.Result.Gateway.MerchantId,
                CallbackUrl = fatcory.Result.Gateway.PostBackUrl,
                Url = fatcory.Result.Gateway.Url
            };
            var createTrans = await fatcory.Result.Service.CreateTrasaction(transModel, null);
            if (!createTrans.IsSuccessful) return Json(new Response<AddOrderReponse> { Message = createTrans.Message, Result = new AddOrderReponse { OrderId = addOrder.Result.Order.OrderId } });
            var addPayment = await _paymentService.Add(transModel, createTrans.Result.TransactionId, fatcory.Result.Gateway.PaymentGatewayId);
            if (!addPayment.IsSuccessful) return Json(new Response<AddOrderReponse> { Message = addPayment.Message, Result = new AddOrderReponse { OrderId = addOrder.Result.Order.OrderId } });
            return Json(new Response<AddOrderReponse>
            {
                IsSuccessful = true,
                Result = new AddOrderReponse
                {
                    OrderId = addOrder.Result.Order.OrderId,
                    Url = createTrans.Result.GatewayUrl,
                    BasketChanged = addOrder.Result.IsChanged,
                    ChangedProducts = addOrder.Result.Order.OrderDetails.Select(x => new ProductDTO
                    {
                        Id = x.ProductId,
                        Discount = x.DiscountPercent ?? 0,
                        Price = x.Price,
                        Count = x.Count
                    })
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddTempBasket([FromBody]TempOrderDTO model)
        {
            var findUser = await _userService.FindAsync(model.UserToken);
            if (!findUser.IsSuccessful) return Json(new Response<string> { Message = Strings.InvalidToken });
            var addOrder = await _orderService.AddTempBasket(model);
            if (!addOrder.IsSuccessful) return Json(new Response<AddOrderReponse> { Message = addOrder.Message });
            var fatcory = await _gatewayFectory.GetInsance(int.Parse(_configuration["DefaultGatewayId"]));
            var transModel = new CreateTransactionRequest
            {
                OrderId = addOrder.Result.OrderId,
                Amount = addOrder.Result.TotalPriceAfterDiscount,
                MobileNumber = findUser.Result.MobileNumber.ToString(),
                ApiKey = fatcory.Result.Gateway.MerchantId,
                CallbackUrl = fatcory.Result.Gateway.PostBackUrl,
                Url = fatcory.Result.Gateway.Url
            };
            var createTrans = await fatcory.Result.Service.CreateTrasaction(transModel, null);
            if (!createTrans.IsSuccessful) return Json(new Response<AddOrderReponse> { Message = createTrans.Message, Result = new AddOrderReponse { OrderId = addOrder.Result.OrderId } });
            var addPayment = await _paymentService.Add(transModel, createTrans.Result.TransactionId, fatcory.Result.Gateway.PaymentGatewayId);
            if (!addPayment.IsSuccessful) return Json(new Response<AddOrderReponse> { Message = addPayment.Message, Result = new AddOrderReponse { OrderId = addOrder.Result.OrderId } });
            return Json(new Response<AddOrderReponse>
            {
                IsSuccessful = true,
                Result = new AddOrderReponse
                {
                    OrderId = addOrder.Result.OrderId,
                    Url = createTrans.Result.GatewayUrl
                }
            });
        }

        [HttpGet]
        public IActionResult ShowResult(Response<string> model)
        {
            return Redirect($"{_configuration["ShowPaymentResult:ReactUrl"]}{Convert.ToByte(model.IsSuccessful)}/{model.Result}");
        }

    }
}