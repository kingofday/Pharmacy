using System;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Dashboard.Controllers
{
    //[AuthorizationFilter]
    public class DrugController : Controller
    {
        private readonly IDrugService _DrugSrv;
        private readonly IConfiguration _configuration;
        private readonly IDrugCategoryService _DrugCategorySrv;
        public DrugController(IDrugService DrugSrv, IConfiguration configuration, IDrugCategoryService DrugCategorySrv)
        {
            _DrugSrv = DrugSrv;
            _configuration = configuration;
            _DrugCategorySrv = DrugCategorySrv;
        }

        [NonAction]
        private List<SelectListItem> GetCategories()
        {
            var categories = _DrugCategorySrv.Get(new DrugCategorySearchFilter());
            if (categories.Items == null) return new List<SelectListItem>();
            return categories.Items.Select(x => new SelectListItem
            {
                Value = x.DrugCategoryId.ToString(),
                Text = x.Name
            }).ToList();
        }

        [HttpGet]
        public virtual JsonResult Add()
        {
            ViewBag.Categories = GetCategories();

            return Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Drug}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Drug()),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Add([FromServices]IWebHostEnvironment env, DrugAddModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.BaseDomain = _configuration["CustomSettings:BaseUrl"];
            model.Root = env.WebRootPath;
            var add = await _DrugSrv.AddAsync(model);
            return Json(new { add.IsSuccessful, add.Message });
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _DrugSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.NotFound });
            ViewBag.Categories = GetCategories();
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Drug}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmit = false
            });
        }
        [HttpPost]
        public virtual async Task<JsonResult> Update([FromServices]IWebHostEnvironment env, DrugAddModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.BaseDomain = _configuration["CustomSettings:BaseUrl"];
            model.Root = env.WebRootPath;
            var update = await _DrugSrv.UpdateAsync(model);
            return Json(new { update.IsSuccessful, update.Message });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete([FromServices]IWebHostEnvironment env, int id) => Json(await _DrugSrv.DeleteAsync(_configuration["CustomSettings:BaseUrl"], env.WebRootPath, id));


        [HttpGet]
        public virtual ActionResult Manage(DrugSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_DrugSrv.Get(filter));
            else return PartialView("Partials/_List", _DrugSrv.Get(filter));
        }

        [HttpPost, AuthEqualTo("Drug", "Delete")]
        public virtual async Task<JsonResult> DeleteAsset([FromServices]IDrugAssetService DrugAssetSerive, int assetId) => Json(await DrugAssetSerive.DeleteAsync(assetId));
        //[HttpGet, AuthEqualTo("DrugInRole", "Add")]
        //public virtual JsonResult Search(string q)
        //    => Json(_DrugSrv.Search(q).ToSelectListItems());
    }
}