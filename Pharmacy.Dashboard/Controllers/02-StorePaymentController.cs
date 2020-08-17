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
    [AuthorizationFilter]
    public partial class StorePaymentController : Controller
    {
        private readonly IPaymentService _paymentSrv;
        private readonly IDrugStoreService _storeSrv;

        public StorePaymentController(IPaymentService paymentSrv, IDrugStoreService storeSrv)
        {
            _paymentSrv = paymentSrv;
            _storeSrv = storeSrv;
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetStores() => _storeSrv.GetAll(User.GetUserId()).Select(x => new SelectListItem
        {
            Text = x.User.FullName,
            Value = x.DrugStoreId.ToString()
        }).ToList();

        [HttpGet, AuthEqualTo("StorePayment", "Manage")]
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

        //[HttpPost]
        //public virtual async Task<JsonResult> Delete(int id) => Json(await _OrderSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(PaymentSearchFilter filter)
        {
            ViewBag.WithoutAddButton = true;
            ViewBag.Stores = GetStores();
            filter.UserId = User.GetUserId();
            if (!Request.IsAjaxRequest()) return View(_paymentSrv.Get(filter));
            else return PartialView("Partials/_List", _paymentSrv.Get(filter));
        }

    }
}