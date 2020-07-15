using Elk.Http;
using Elk.Core;
using Elk.AspNetCore;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using DomainString = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Dashboard.Controllers
{
    public class StoreController : Controller
    {
        readonly IStoreService _storeSrv;
        readonly IConfiguration _configuration;
        public StoreController(IConfiguration configuration,IStoreService storeSrv)
        {
            _storeSrv = storeSrv;
            _configuration = configuration;
        }

        [HttpGet, AuthEqualTo("Store","Manage")]
        public virtual JsonResult Search(string q) => Json(_storeSrv.Search(q, null).ToSelectListItems());

        [HttpGet]
        public virtual async Task<JsonResult> Update([FromServices]IAddressService addrSrv, int id)
        {
            var store = await _storeSrv.FindAsync(id);
            if (!store.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound });
            //var model = new StoreUpdateModel().CopyFrom(store.Result);
            if (store.Result.AddressId != null)
            {
                var addr = await addrSrv.FindAsync(store.Result.AddressId ?? 0);
                store.Result.Address = addr.IsSuccessful ? addr.Result : new Address();
            }
            else store.Result.Address = new Address();
            store.Result.PharmacyUrl = $"{_configuration["CustomSettings:ReactBaseUrl"]}/store/{id}";
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Store}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", store.Result),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update([FromServices]IWebHostEnvironment env, DrugStoreAdminUpdateModel model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.Root = env.WebRootPath;
            model.BaseDomain = _configuration["CustomSettings:BaseUrl"];
            return Json(await _storeSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _storeSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(StoreSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_storeSrv.Get(filter));
            else return PartialView("Partials/_List", _storeSrv.Get(filter));
        }

        [HttpPost, AuthEqualTo("Store", "Update")]
        public virtual async Task<IActionResult> DeleteLogo([FromServices]IWebHostEnvironment env, int id) => Json(await _storeSrv.DeleteFile(_configuration["CustomSettings:BaseUrl"], env.WebRootPath, id));


    }
}