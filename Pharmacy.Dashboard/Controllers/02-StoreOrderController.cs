using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.Http;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using DomainString = Pharmacy.Domain.Resource.Strings;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public partial class StoreOrderController : Controller
    {
        private readonly IOrderService _OrderSrv;
        private readonly IDrugStoreService _storeSrv;

        public StoreOrderController(IOrderService OrderSrv, IDrugStoreService storeSrv)
        {
            _OrderSrv = OrderSrv;
            _storeSrv = storeSrv;
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetStores() => _storeSrv.GetAll(User.GetUserId()).Select(x => new SelectListItem
        {
            Text = x.User.FullName,
            Value = x.DrugStoreId.ToString()
        }).ToList();

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _OrderSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Order) });
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Order}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "StoreOrder"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Order model)
        {
            return Json(await _OrderSrv.UpdateStatusAsync(model.OrderId, model.OrderStatus));
        }

        [HttpGet, AuthEqualTo("StoreOrder", "Update")]
        public virtual async Task<JsonResult> Details(int id)
        {
            var findRep = await _OrderSrv.GetDetails(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Order) });

            return Json(new Modal
            {
                Title = $"{Strings.Details} {DomainString.Order}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Details", findRep.Result),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _OrderSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(OrderSearchFilter filter)
        {
            ViewBag.WithoutAddButton = true;
            ViewBag.Stores = GetStores();
            filter.UserId = User.GetUserId();
            if (!Request.IsAjaxRequest()) return View(_OrderSrv.Get(filter));
            else return PartialView("Partials/_List", _OrderSrv.Get(filter));
        }

    }
}