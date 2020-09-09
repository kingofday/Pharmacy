using Elk.Http;
using Elk.AspNetCore;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using DomainString = Pharmacy.Domain.Resource.Strings;
using Elk.Core;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public class DrugStoreController : Controller
    {
        readonly IDrugStoreService _DrugSrv;
        readonly IConfiguration _configuration;
        public DrugStoreController(IConfiguration configuration, IDrugStoreService DrugSrv)
        {
            _DrugSrv = DrugSrv;
            _configuration = configuration;
        }

        [HttpGet, AuthEqualTo("DrugStore", "Manage")]
        public virtual JsonResult Search(string q) => Json(_DrugSrv.Search(q, null).ToSelectListItems());

        [HttpGet]
        public virtual JsonResult Add([FromServices] IAddressService addrSrv, int id)
        {
            return Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Pharmacy}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new DrugStoreAdminModel
                {
                    Address = new DrugStoreAddress()
                }),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Add([FromServices] IWebHostEnvironment env, DrugStoreAdminModel model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.AppDir = env.WebRootPath;
            var add = await _DrugSrv.AddAsync(model);
            return Json(new Response<string> { Message = add.Message, IsSuccessful = add.IsSuccessful });
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update([FromServices] IWebHostEnvironment env, int id)
        {
            var drug = await _DrugSrv.FindAsync(id);
            if (!drug.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Pharmacy}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new DrugStoreAdminModel
                {
                    Name = drug.Result.Name,
                    Attachments = drug.Result.Attachments,
                    AppDir = env.WebRootPath,
                    IsActive = drug.Result.IsActive,
                    DrugStoreId = id,
                    Status = drug.Result.Status,
                    UserId = drug.Result.UserId,
                    User = drug.Result.User,
                    Address = drug.Result.Address
                }),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update([FromServices] IWebHostEnvironment env, DrugStoreAdminModel model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.AppDir = env.WebRootPath;
            var update = await _DrugSrv.UpdateAsync(model);
            return Json(new Response<string> { IsSuccessful = update.IsSuccessful, Message = update.Message });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete([FromServices] IWebHostEnvironment env, int id) => Json(await _DrugSrv.DeleteAsync(id, env.WebRootPath));

        [HttpGet]
        public virtual ActionResult Manage(DrugStoreSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_DrugSrv.Get(filter));
            else return PartialView("Partials/_List", _DrugSrv.Get(filter));
        }

        [HttpPost, AuthEqualTo("Drug", "Update")]
        public virtual async Task<IActionResult> DeleteAttachment([FromServices] IWebHostEnvironment env, int attchId) => Json(await _DrugSrv.DeleteFile(env.WebRootPath, attchId));


    }
}