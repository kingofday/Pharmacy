using System;
using Elk.Core;
using Elk.Http;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pharmacy.Dashboard.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using DomainString = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public partial class Store_OrderController : Controller
    {
        private readonly IOrderService _OrderSrv;
        private readonly IDrugStoreService _storeSrv;

        public Store_OrderController(IOrderService OrderSrv, IDrugStoreService storeSrv)
        {
            _OrderSrv = OrderSrv;
            _storeSrv = storeSrv;
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetStores() => _storeSrv.GetAll(User.GetUserId()).Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.DrugStoreId.ToString()
        }).ToList();
        [NonAction]
        private IEnumerable<SelectListItem> GetStatus()
        =>  new List<SelectListItem> {
                new SelectListItem
                {
                    Text = OrderStatus.InProcessing.GetDescription(),
                    Value= OrderStatus.InProcessing.ToString()
                },
                new SelectListItem
                {
                    Text = OrderStatus.Accepted.GetDescription(),
                    Value= OrderStatus.Accepted.ToString()
                },
                new SelectListItem
                {
                    Text = OrderStatus.WaitForDelivery.GetDescription(),
                    Value= OrderStatus.WaitForDelivery.ToString()
                }
            };
        

        [HttpGet]
        public virtual async Task<JsonResult> Update(Guid id)
        {
            var findRep = await _OrderSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Order) });
            ViewBag.Status = GetStatus();
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Order}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "Store_Order"),
                AutoClose = true,
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Store_OrderUpdateModel model)
        {
            var update = await _OrderSrv.Store_UpdateAsync(model);
            return Json(new { update.IsSuccessful, update.Message });
        }

        [HttpGet, AuthEqualTo("Store_Order", "Update")]
        public virtual async Task<JsonResult> Details(Guid id)
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
        public virtual async Task<JsonResult> Delete(Guid id) => Json(await _OrderSrv.DeleteAsync(id));

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