using Elk.Core;
using Elk.Http;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using DomainString = Pharmacy.Domain.Resource.Strings;
using Microsoft.Extensions.Options;

namespace Pharmacy.Dashboard.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionService _prescriptionSrv;
        private readonly IConfiguration _configuration;
        private readonly IUnitService _unitSrv;
        private readonly IOrderService _orderSrv;
        public PrescriptionController(IPrescriptionService PrescriptionSrv,
            IConfiguration configuration,
            IUnitService unitSrv,
            IOrderService orderSrv)
        {
            _prescriptionSrv = PrescriptionSrv;
            _configuration = configuration;
            _unitSrv = unitSrv;
            _orderSrv = orderSrv;
        }

        //[HttpGet]
        //public virtual JsonResult Add()
        //{
        //    return Json(new Modal
        //    {
        //        Title = $"{Strings.Add} {DomainString.Prescription}",
        //        Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Prescription()),
        //        AutoSubmit = false
        //    });
        //}

        //[HttpPost]
        //public virtual async Task<JsonResult> Add([FromServices] IWebHostEnvironment env, PrescriptionAddModel model)
        //{
        //    if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
        //    model.AppDir = env.WebRootPath;
        //    var add = await _prescriptionSrv.AddAsync(model);
        //    return Json(new { add.IsSuccessful, add.Message });
        //}

        [HttpGet, AuthorizationFilter]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _prescriptionSrv.FindDetailsAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.NotFound });
            ViewBag.PrescriptionId = id;
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Prescription}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update([FromServices] IWebHostEnvironment env, Prescription model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            var update = await _prescriptionSrv.UpdateAsync(model);
            return Json(new { update.IsSuccessful, update.Message });
        }

        //[HttpPost]
        // public virtual async Task<JsonResult> Delete([FromServices] IWebHostEnvironment env, int id) => Json(await _prescriptionSrv.DeleteAsync(env.WebRootPath, id));


        [HttpGet, AuthorizationFilter]
        public virtual ActionResult Manage(PrescriptionSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_prescriptionSrv.Get(filter));
            else return PartialView("Partials/_List", _prescriptionSrv.Get(filter));
        }

        [HttpPost, AuthEqualTo("Prescription", "Update")]
        public virtual async Task<ActionResult> DeleteItem(int itemId)
        {
            var delete = await _prescriptionSrv.DeleteItem(itemId);
            return Json(new Response<string>
            {
                IsSuccessful = delete.IsSuccessful,
                Message = delete.Message,
                Result = delete.IsSuccessful ? ControllerExtension.RenderViewToString(this, "Partials/_Items", delete.Result) : string.Empty
            });
        }

        [HttpPost, AuthEqualTo("Prescription", "Update")]
        public virtual async Task<ActionResult> SendLink([FromServices] IOptions<DashboardCustomSetting> settings, int id)
        {
            var delete = await _prescriptionSrv.SendLink(id, settings.Value.ReactTempBasketUrl);
            return Json(new Response<string>
            {
                IsSuccessful = delete.IsSuccessful,
                Message = delete.Message,
                Result = delete.IsSuccessful ? ControllerExtension.RenderViewToString(this, "Partials/_Items", delete.Result) : string.Empty
            });
        }

        //[HttpPost, AuthEqualTo("Prescription", "Delete")]
        //public virtual async Task<JsonResult> DeleteAttachment([FromServices] IWebHostEnvironment env, int attchId) => Json(await _prescriptionSrv.DeleteAttachment(env.WebRootPath, attchId));

        //[HttpPost, AuthEqualTo("Prescription", "Delete")]
        //public virtual async Task<JsonResult> DeleteProp(int propId) => Json(await _prescriptionSrv.DeleteProp(propId));
        [HttpPost, AllowAnonymous]
        public virtual async Task<ActionResult> AddByApi([FromServices] IWebHostEnvironment env, [FromForm] AddPrescriptionModel model)
        {
            model.AppDir = env.WebRootPath;
            return Json(await _prescriptionSrv.Add(model));
        }
    }
}