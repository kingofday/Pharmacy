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
    public class ProductController : Controller
    {
        private readonly IDrugService _productSrv;
        private readonly IConfiguration _configuration;
        private readonly IDrugCategoryService _productCategorySrv;
        public ProductController(IDrugService productSrv, IConfiguration configuration, IDrugCategoryService productCategorySrv)
        {
            _productSrv = productSrv;
            _configuration = configuration;
            _productCategorySrv = productCategorySrv;
        }

        [NonAction]
        private List<SelectListItem> GetCategories()
        {
            var categories = _productCategorySrv.Get(new ProductCategorySearchFilter());
            if (categories.Items == null) return new List<SelectListItem>();
            return categories.Items.Select(x => new SelectListItem
            {
                Value = x.ProductCategoryId.ToString(),
                Text = x.Name
            }).ToList();
        }

        [HttpGet]
        public virtual JsonResult Add()
        {
            ViewBag.Categories = GetCategories();

            return Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Product}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Product()),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Add([FromServices]IWebHostEnvironment env, ProductAddModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.BaseDomain = _configuration["CustomSettings:BaseUrl"];
            model.Root = env.WebRootPath;
            var add = await _productSrv.AddAsync(model);
            return Json(new { add.IsSuccessful, add.Message });
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _productSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.NotFound });
            ViewBag.Categories = GetCategories();
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Product}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmit = false
            });
        }
        [HttpPost]
        public virtual async Task<JsonResult> Update([FromServices]IWebHostEnvironment env, ProductAddModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.BaseDomain = _configuration["CustomSettings:BaseUrl"];
            model.Root = env.WebRootPath;
            var update = await _productSrv.UpdateAsync(model);
            return Json(new { update.IsSuccessful, update.Message });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete([FromServices]IWebHostEnvironment env, int id) => Json(await _productSrv.DeleteAsync(_configuration["CustomSettings:BaseUrl"], env.WebRootPath, id));


        [HttpGet]
        public virtual ActionResult Manage(ProductSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_productSrv.Get(filter));
            else return PartialView("Partials/_List", _productSrv.Get(filter));
        }

        [HttpPost, AuthEqualTo("Product", "Delete")]
        public virtual async Task<JsonResult> DeleteAsset([FromServices]IProductAssetService productAssetSerive, int assetId) => Json(await productAssetSerive.DeleteAsync(assetId));
        //[HttpGet, AuthEqualTo("ProductInRole", "Add")]
        //public virtual JsonResult Search(string q)
        //    => Json(_productSrv.Search(q).ToSelectListItems());
    }
}