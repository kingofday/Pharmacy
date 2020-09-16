using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Pharmacy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryProviderController : ControllerBase
    {
        readonly IDeliveryProviderService _deliveryProviderSrv;
        readonly IOrderService _orderSrv;
        readonly IGatewayFactory _gatewayFectory;
        readonly APICustomSetting _settings;
        public DeliveryProviderController(IDeliveryProviderService deliveryProviderSrv,
            IOrderService orderSrv,
            IGatewayFactory gatewayFactory,
            IOptions<APICustomSetting> settings)
        {
            _deliveryProviderSrv = deliveryProviderSrv;
            _orderSrv = orderSrv;
            _gatewayFectory = gatewayFactory;
            _settings = settings.Value;
        }

        [HttpGet]
        public ActionResult<IResponse<List<DeliveryDTO>>> Get() => _deliveryProviderSrv.GetAllAsDTO();

        [HttpGet, Route("{id:Guid}")]
        public ActionResult<IResponse<GetDeliveryPriceDTO>> GetPrice(Guid id) => _orderSrv.GetDeliveryPrice(id);

        [HttpPost, Route("{id:Guid}")]
        public async Task<ActionResult<IResponse<string>>> PayPrice(Guid id)
        {
            var chk = await _orderSrv.CheckBeforeDeliveryPrice(id);
            if (!chk.IsSuccessful) return new Response<string> { Message = chk.Message };
            var fatcory = await _gatewayFectory.GetInsance(_settings.DefaultGatewayId);
            var transModel = new CreateTransactionRequest
            {
                OrderId = chk.Result.Order.OrderId,
                PaymentType = PaymentType.DeliveryPrice,
                GatewayId = _settings.DefaultGatewayId,
                Amount = chk.Result.price,
                MobileNumber = chk.Result.Order.Address.User.MobileNumber.ToString(),
                ApiKey = fatcory.Result.Gateway.MerchantId,
                CallbackUrl = fatcory.Result.Gateway.PostBackUrl,
                Url = fatcory.Result.Gateway.Url
            };
            var createTrans = await fatcory.Result.Service.CreateTransaction(transModel, null);
            if (!createTrans.IsSuccessful) return new Response<string> { Message = createTrans.Message };
            return new Response<string>
            {
                IsSuccessful = createTrans.IsSuccessful,
                Message = createTrans.Message,
                Result = createTrans.Result.GatewayUrl
            };
        }
    }
}