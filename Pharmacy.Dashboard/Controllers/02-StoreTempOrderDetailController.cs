using System;
using Elk.Http;
using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using DomainString = Pharmacy.Domain.Resource.Strings;
using System.Net.Http;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public partial class StoreTempBasketItemController : Controller
    {
        //private readonly ITempBasketItemService _TempBasketItemSrv;
        //private readonly IConfiguration _configuration;

        //public StoreTempBasketItemController(ITempBasketItemService TempBasketItemSrv, IConfiguration configuration)
        //{
        //    _TempBasketItemSrv = TempBasketItemSrv;
        //    _configuration = configuration;
        //}


        //[HttpGet]
        //public virtual JsonResult Add()
        //    => Json(new Modal
        //    {
        //        Title = $"{Strings.Add} {DomainString.Basket}",
        //        Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new TempBasketItem()),
        //        AutoSubmit = false
        //    });

        //[HttpPost]
        //public virtual async Task<JsonResult> Add([FromBody]IList<TempBasketItem> items)
        //{
        //    if (items == null || items.Count == 0) return Json(new { IsSuccessful = false, Message = Strings.ThereIsNoRecord });
        //    if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
        //    var add = await _TempBasketItemSrv.AddRangeAsync(items);
        //    if (!add.IsSuccessful) return Json(add);
        //    var url = $"{_configuration["CustomSettings:ReactTempBasketUrl"]}/{add.Result}";
        //    return Json(new Response<string>
        //    {
        //        IsSuccessful = true,
        //        Result = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Result", new TempBasketItemResultModel { Url = url })
        //    });
        //}

        //[HttpGet, AuthEqualTo("StoreTempBasketItem", "Add")]
        //public virtual JsonResult Details(Guid id)
        //{
        //    ViewBag.BasketUrl = $"{_configuration["CustomSettings:ReactTempBasketUrl"]}/{id}";
        //    return Json(new Modal
        //    {
        //        Title = $"{Strings.Details} {DomainString.Basket}",
        //        AutoSubmitBtnText = Strings.Edit,
        //        Body = ControllerExtension.RenderViewToString(this, "Partials/_Details", _TempBasketItemSrv.GetDetails(id)),
        //        AutoSubmit = false
        //    });
        //}

        //[HttpPost]
        //public virtual async Task<JsonResult> Update(TempBasketItem model)
        //{
        //    if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
        //    return Json(await _TempBasketItemSrv.UpdateAsync(model));
        //}

        //[HttpPost]
        //public virtual async Task<JsonResult> Delete(Guid id) => Json(await _TempBasketItemSrv.DeleteAsync(id));

        //[HttpGet]
        //public virtual ActionResult Manage(TempBasketItemSearchFilter filter)
        //{
        //    if (!Request.IsAjaxRequest()) return View(_TempBasketItemSrv.Get(filter));
        //    else return PartialView("Partials/_List", _TempBasketItemSrv.Get(filter));
        //}

        //[HttpPost, AuthEqualTo("StoreTempBasketItem", "Add")]
        //public virtual async Task<JsonResult> Notify([FromServices]INotificationService notifSrv, [FromBody]TempBasketItemResultModel model)
        //{
        //    var notify = await notifSrv.NotifyAsync(new NotificationDto
        //    {
        //        Content = Strings.TempBasketText.Fill(model.Url),
        //        MobileNumber = long.Parse(model.MobileNumber),
        //        Type = EventType.Subscription
        //    });
        //    return Json(new { IsSuccessful = notify.IsSuccessful, Message = notify.IsSuccessful ? string.Empty : Strings.Error });
        //}
    }
}