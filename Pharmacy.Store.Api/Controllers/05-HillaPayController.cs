using Elk.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;

namespace Pharmacy.Store.Api.Controllers
{
    public class HillaPayController : Controller
    {
        readonly IOrderService _orderSrv;
        readonly IDeliveryService _deliverySrv;
        readonly IPaymentService _paymentSrv;
        readonly IGatewayService _gatewaySrv;
        readonly IConfiguration _configuration;
        public HillaPayController(IOrderService orderSrv, IPaymentService paymentSrv, IGatewayService gatewaySrv, IConfiguration configuration, IDeliveryService deliverySrv)
        {
            _orderSrv = orderSrv;
            _configuration = configuration;
            _paymentSrv = paymentSrv;
            _gatewaySrv = gatewaySrv;
            _deliverySrv = deliverySrv;
        }

        [HttpPost]
        public async Task<IActionResult> AfterGateway(HillaPayAfterGatewayModel model)
        {
            var controller = _configuration["ShowPaymentResult:Controller"];
            var action = _configuration["ShowPaymentResult:Action"];
            if (model.Status.status != 400) return RedirectToAction(action, controller, new Response<string> { IsSuccessful = false, Result = model.result_transaction_callback.transaction_id });
            else
            {
                var findPayment = await _paymentSrv.FindAsync(model.result_transaction_callback.transaction_id);
                if (!findPayment.IsSuccessful) return RedirectToAction(action, controller, new Response<string> { IsSuccessful = false, Result = model.result_transaction_callback.transaction_id });
                var verify = await _orderSrv.Verify(findPayment.Result, new object[1] { model.result_transaction_callback.rrn });
                if (verify.IsSuccessful) await _deliverySrv.Add(findPayment.Result.OrderId);
                return RedirectToAction(action, controller, new Response<string> { IsSuccessful = verify.IsSuccessful, Result = model.result_transaction_callback.transaction_id });
            }

        }
    }
}