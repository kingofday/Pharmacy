﻿using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Pharmacy.API.Controllers
{
    [ApiController, Route("[controller]")]
    public class HillaPayController : ControllerBase
    {
        readonly IOrderService _orderSrv;
        readonly IPaymentService _paymentSrv;
        readonly CustomSetting _settings;
        public HillaPayController(IOrderService orderSrv,
            IPaymentService paymentSrv,
            IOptions<CustomSetting> settings)
        {
            _orderSrv = orderSrv;
            _paymentSrv = paymentSrv;
            _settings = settings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> AfterGateway([FromForm]HillaPayAfterGatewayModel model)
        {
            if (model.Status.status != 400) return RedirectToAction(_settings.ShowPaymentResult.Action,_settings.ShowPaymentResult.Controller, new Response<string> { IsSuccessful = false, Result = model.result_transaction_callback.transaction_id });
            else
            {
                var findPayment = await _paymentSrv.FindAsync(model.result_transaction_callback.transaction_id);
                if (!findPayment.IsSuccessful) return RedirectToAction(_settings.ShowPaymentResult.Action, _settings.ShowPaymentResult.Controller, new Response<string> { IsSuccessful = false, Result = model.result_transaction_callback.transaction_id });
                var verify = await _orderSrv.Verify(findPayment.Result, new object[1] { model.result_transaction_callback.rrn });
                return RedirectToAction(_settings.ShowPaymentResult.Action, _settings.ShowPaymentResult.Controller, new Response<string> { IsSuccessful = verify.IsSuccessful, Result = model.result_transaction_callback.transaction_id });
            }

        }
    }
}