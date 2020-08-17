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
    public class DrugStoreController : Controller
    {
        readonly IDrugStoreService _DrugSrv;
        readonly IConfiguration _configuration;
        public DrugStoreController(IConfiguration configuration, IDrugStoreService DrugSrv)
        {
            _DrugSrv = DrugSrv;
            _configuration = configuration;
        }

        [HttpGet, AuthEqualTo("Drug","Manage")]
        public virtual JsonResult Search(string q) => Json(_DrugSrv.Search(q, null).ToSelectListItems());

        [HttpGet]
        public virtual async Task<JsonResult> Update([FromServices]IAddressService addrSrv, int id)
        {
            var drug = await _DrugSrv.FindAsync(id);
            if (!drug.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Drug}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", drug.Result),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update([FromServices]IWebHostEnvironment env, DrugStoreAdminUpdateModel model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.Root = env.WebRootPath;
            model.BaseDomain = _configuration["CustomSettings:BaseUrl"];
            return Json(await _DrugSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _DrugSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(DrugStoreSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_DrugSrv.Get(filter));
            else return PartialView("Partials/_List", _DrugSrv.Get(filter));
        }

        [HttpPost, AuthEqualTo("Drug", "Update")]
        public virtual async Task<IActionResult> DeleteLogo([FromServices]IWebHostEnvironment env, int id) => Json(await _DrugSrv.DeleteFile(_configuration["CustomSettings:BaseUrl"], env.WebRootPath, id));


    }
}