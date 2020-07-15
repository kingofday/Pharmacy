using Elk.Core;
using Elk.Http;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using DomainString = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Dashboard.Controllers
{
    //[AuthorizationFilter]
    public partial class PaymentController : Controller
    {
        private readonly IPaymentService _paymentSrv;
        private readonly IStoreService _storeSrv;

        public PaymentController(IPaymentService paymentSrv, IStoreService storeSrv)
        {
            _paymentSrv = paymentSrv;
            _storeSrv = storeSrv;
        }

        [HttpGet, AuthEqualTo("Payment", "Manage")]
        public virtual async Task<JsonResult> Details(int id)
        {
            ViewBag.WithoutAddButton = true;
            var findRep = await _paymentSrv.GetDetails(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Order) });

            return Json(new Modal
            {
                Title = $"{Strings.Details} {DomainString.Order}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Details", findRep.Result),
                AutoSubmit = false
            });
        }

        [HttpGet]
        public virtual ActionResult Manage(PaymentSearchFilter filter)
        {
            ViewBag.WithoutAddButton = true;
            if (!Request.IsAjaxRequest()) return View(_paymentSrv.Get(filter));
            else return PartialView("Partials/_List", _paymentSrv.Get(filter));
        }

    }
}