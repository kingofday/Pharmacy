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
using DomainString = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public class StoreStoreController : Controller
    {
        private readonly IDrugStoreService _storeSrv;
        readonly IConfiguration _configuration;
        public StoreStoreController(IDrugStoreService storeSrv, IConfiguration configuration)
        {
            _storeSrv = storeSrv;
            _configuration = configuration;
        }


        [HttpGet]
        public virtual async Task<JsonResult> Update([FromServices]IAddressService addrSrv, int id)
        {
            var chk = await _storeSrv.CheckOwner(id, User.GetUserId());
            if (!chk) return Json(new { IsSuccessful = false, Message = Strings.AccessDenied });
            var store = await _storeSrv.FindAsync(id);
            if (!store.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound });
            var model = new DrugStoreUpdateModel().CopyFrom(store.Result);
            if (store.Result.Address != null)
            {
                model.Address.Latitude = store.Result.Address.Latitude;
                model.Address.Longitude = store.Result.Address.Longitude;
                model.Address.Details = store.Result.Address.Details;
            }

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Pharmacy}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", store.Result),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update([FromServices]IWebHostEnvironment env, DrugStoreUpdateModel model)
        {
            var chk = await _storeSrv.CheckOwner(model.DrugStoreId, User.GetUserId());
            if (!chk) return Json(new { IsSuccessful = false, Message = Strings.AccessDenied });
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.Root = env.WebRootPath;
            model.AppDir = _configuration["CustomSettings:BaseUrl"];
            return Json(await _storeSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete([FromServices] IWebHostEnvironment env, int id)
        {
            var chk = await _storeSrv.CheckOwner(id, User.GetUserId());
            if (!chk) return Json(new { IsSuccessful = false, Message = Strings.AccessDenied });
            return Json(await _storeSrv.DeleteAsync(id, env.WebRootPath ));
        }

        [HttpGet]
        public virtual ActionResult Manage(DrugStoreSearchFilter filter)
        {
            filter.UserId = User.GetUserId();
            if (!Request.IsAjaxRequest()) return View(_storeSrv.Get(filter));
            else return PartialView("Partials/_List", _storeSrv.Get(filter));
        }

        [HttpPost, AuthEqualTo("StoreStore", "Update")]
        public virtual async Task<IActionResult> DeleteLogo([FromServices]IWebHostEnvironment env, int id) => Json(await _storeSrv.DeleteFile(env.WebRootPath, id));

        [HttpGet, AuthEqualTo("StoreStore", "Update")]
        public virtual JsonResult Search(string q)
            => Json(_storeSrv.Search(q, User.GetUserId()).ToSelectListItems());
    }
}