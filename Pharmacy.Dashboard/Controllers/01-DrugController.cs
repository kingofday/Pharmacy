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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using DomainString = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public class DrugController : Controller
    {
        private readonly IDrugService _DrugSrv;
        private readonly IConfiguration _configuration;
        private readonly IDrugCategoryService _DrugCategorySrv;
        private readonly IUnitService _unitSrv;
        public DrugController(IDrugService DrugSrv, IConfiguration configuration,
            IDrugCategoryService DrugCategorySrv,
            IUnitService unitSrv)
        {
            _DrugSrv = DrugSrv;
            _configuration = configuration;
            _DrugCategorySrv = DrugCategorySrv;
            _unitSrv = unitSrv;
        }

        [NonAction]
        private List<SelectListItem> GetCategories()
        {
            var categories = _DrugCategorySrv.Get(new DrugCategorySearchFilter { PageSize = 100 });
            if (categories.Items == null) return new List<SelectListItem>();
            return categories.Items.Select(x => new SelectListItem
            {
                Value = x.DrugCategoryId.ToString(),
                Text = x.Name
            }).ToList();
        }

        [NonAction]
        private List<SelectListItem> GetUnits()
        {
            var units = _unitSrv.Get(new UnitSearchFilter { PageSize = 100 });
            if (units.Items == null) return new List<SelectListItem>();
            return units.Items.Select(x => new SelectListItem
            {
                Value = x.UnitId.ToString(),
                Text = x.Name
            }).ToList();

        }
        [HttpGet]
        public virtual JsonResult Add()
        {
            ViewBag.Categories = GetCategories();
            ViewBag.Units = GetUnits();
            return Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Drug}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Drug { IsActive = true }),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Add([FromServices] IWebHostEnvironment env, DrugAddModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.AppDir = env.WebRootPath;
            var add = await _DrugSrv.AddAsync(model);
            return Json(new { add.IsSuccessful, add.Message });
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _DrugSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.NotFound });
            ViewBag.Categories = GetCategories();
            ViewBag.Units = GetUnits();
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Drug}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmit = false
            });
        }
        [HttpPost]
        public virtual async Task<JsonResult> Update([FromServices] IWebHostEnvironment env, DrugAddModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.AppDir = env.WebRootPath;
            var update = await _DrugSrv.UpdateAsync(model);
            return Json(new { update.IsSuccessful, update.Message });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete([FromServices] IWebHostEnvironment env, int id) => Json(await _DrugSrv.DeleteAsync(env.WebRootPath, id));


        [HttpGet]
        public virtual ActionResult Manage(DrugSearchFilter filter)
        {
            ViewBag.Categories = GetCategories();
            if (!Request.IsAjaxRequest()) return View(_DrugSrv.Get(filter));
            else return PartialView("Partials/_List", _DrugSrv.Get(filter));
        }

        [HttpPost, AuthEqualTo("Drug", "Delete")]
        public virtual async Task<JsonResult> DeleteAttachment([FromServices] IWebHostEnvironment env, int attchId) => Json(await _DrugSrv.DeleteAttachment(env.WebRootPath, attchId));

        [HttpPost, AuthEqualTo("Drug", "Delete")]
        public virtual async Task<JsonResult> DeleteProp(int propId) => Json(await _DrugSrv.DeleteProp(propId));
        //[HttpGet, AuthEqualTo("DrugInRole", "Add")]
        //public virtual JsonResult Search(string q)
        //    => Json(_DrugSrv.Search(q).ToSelectListItems());

        [HttpGet, AuthEqualTo("Drug", "Manage")]
        public virtual JsonResult Search(string q) => Json(_DrugSrv.Search(q).Select(x=>new {
            Text = $"{x.NameFa}({x.UniqueId})",
            Value = x.Id.ToString(),
            x.Price,
            x.DiscountPrice
        }));
    }
}