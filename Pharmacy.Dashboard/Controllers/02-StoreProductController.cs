using Elk.Core;
using Elk.Http;
using System;
using System.Linq;
using Pharmacy.Domain;
using Elk.AspNetCore;
using Pharmacy.Service;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using DomainString = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public class StoreProductController : Controller
    {
        readonly IDrugService _productSerive;
        readonly IDrugCategoryService _productCategorySerive;
        readonly IConfiguration _configuration;
        public StoreProductController(IDrugService productSerive, IDrugCategoryService productCategorySerive, IConfiguration configuration)
        {
            _productSerive = productSerive;
            _productCategorySerive = productCategorySerive;
            _configuration = configuration;
        }

        [NonAction]
        private List<SelectListItem> GetCategories()
        {
            var categories = _productCategorySerive.Get(new ProductCategorySearchFilter());
            if (categories.Items == null) return new List<SelectListItem>();
            return categories.Items.Select(x => new SelectListItem
            {
                Value = x.ProductCategoryId.ToString(),
                Text = x.Name
            }).ToList();
        }

        public IActionResult Manage([FromServices]IStoreService storeSerive, ProductSearchFilter filter)
        {
            var userId = User.GetUserId();
            ViewBag.Stores = storeSerive.GetAll(userId);
            filter.UserId = userId;
            if (!Request.IsAjaxRequest()) return View(_productSerive.Get(filter));
            else return PartialView("Partials/_List", _productSerive.Get(filter));
        }

        [HttpGet, AuthEqualTo("StoreProduct", "Add")]
        public async Task<IActionResult> Post(string username, int pageNumber)
        {
            var getPosts = await _productSerive.GetPosts(username, pageNumber);
            if (!getPosts.IsSuccessful) return Json(getPosts);
            var model = getPosts.Result.Select(x => new PostModel
            {
                Description = x.Description,
                UniqueId = x.UniqueId,
                Assets = x.AssetList
            }).ToList();
            return Json(new
            {
                IsSuccessful = true,
                Result = new
                {
                    Partial = this.RenderViewToString("~/Views/StoreProduct/Partials/_Posts.cshtml", model),
                    Posts = model
                }
            });
        }

        [HttpPost, AuthEqualTo("StoreProduct", "Add")]
        public async Task<IActionResult> AddRange(int storeId, IList<PostModel> posts)
            => Json(await _productSerive.AddRangeAsync(new ProductAddRangeModel
            {
                Posts = posts,
                StoreId = storeId
            }));

        [HttpGet]
        public virtual async Task<JsonResult> Add()
        {
            ViewBag.Categories = GetCategories();
            return Json(new Modal
            {
                IsSuccessful = true,
                Title = $"{Strings.Add} {DomainString.Product}",
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Entity", new Product()),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Add([FromServices]IWebHostEnvironment env, ProductAddModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.BaseDomain = _configuration["CustomSettings:BaseUrl"];
            model.Root = env.WebRootPath;
            var add = await _productSerive.AddAsync(model);
            return Json(new { add.IsSuccessful, add.Message, add.Result });
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _productSerive.FindAsync(id);
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
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            model.BaseDomain = _configuration["CustomSettings:BaseUrl"];
            model.Root = env.WebRootPath;
            var update = await _productSerive.UpdateAsync(model);
            return Json(new { update.IsSuccessful, update.Message });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete([FromServices]IWebHostEnvironment env, int id) => Json(await _productSerive.DeleteAsync(_configuration["CustomSettings:BaseUrl"], env.WebRootPath, id));

        [HttpPost,AuthEqualTo("StoreProduct", "Delete")]
        public virtual async Task<JsonResult> DeleteAsset([FromServices]IProductAssetService productAssetSerive, int assetId) => Json(await productAssetSerive.DeleteAsync(assetId));

        [HttpGet, AuthEqualTo("StoreProduct", "Manage")]
        public virtual JsonResult Search(string q) => Json(_productSerive.Search(q, User.GetUserId()));
    }
}