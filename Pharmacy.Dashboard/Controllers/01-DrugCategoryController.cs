using System;
using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using DomainString = Pharmacy.Domain.Resource.Strings;
using Microsoft.AspNetCore.Authorization;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public class DrugCategoryController : Controller
    {
        private readonly IDrugCategoryService _productCatSrv;

        public DrugCategoryController(IDrugCategoryService DrugCategorySrv)
        {
            _productCatSrv = DrugCategorySrv;
        }

        [NonAction]
        private void GetAddPartial() => ViewBag.EntityPartial = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new DrugCategory());

        [HttpGet]
        public virtual ActionResult Manage(DrugCategorySearchFilter filter)
        {
            GetAddPartial();
            ViewBag.EntityPartial = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new DrugCategory());
            return View(_productCatSrv.GetAll(filter));
        }



        [HttpPost]
        public virtual async Task<JsonResult> Add(DrugCategory model)
        {
            GetAddPartial();
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            var add = await _productCatSrv.AddAsync(model);
            if (!add.IsSuccessful) return Json(add);
            return Json(new Response<string>
            {
                IsSuccessful = true,
                Result = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_List", _productCatSrv.GetAll(new DrugCategorySearchFilter()))
            });
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findProduct = await _productCatSrv.FindAsync(id);
            if (!findProduct.IsSuccessful) return Json(new Response<string> { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Drug) });
            return Json(new Response<string>
            {
                IsSuccessful = true,
                Result = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Entity", findProduct.Result)
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(DrugCategory model)
        {
            GetAddPartial();
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            var update = await _productCatSrv.UpdateAsync(model);
            if (!update.IsSuccessful)
                return Json(update);
            return Json(new
            {
                IsSuccessful = true,
                Result = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_List", _productCatSrv.GetAll(new DrugCategorySearchFilter()))
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id)
        {
            var delete = await _productCatSrv.DeleteAsync(id);
            if (!delete.IsSuccessful)
                return Json(delete);
            return Json(new
            {
                IsSuccessful = true,
                Result = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_List", _productCatSrv.GetAll(new DrugCategorySearchFilter()))
            });
        }


        [HttpGet, AllowAnonymous]
        public virtual JsonResult Search(string q)
            => Json(_productCatSrv.Search(q).ToSelectListItems());
    }
}