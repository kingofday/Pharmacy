using Elk.Core;
using Elk.Http;
using Elk.Cache;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using System.Collections.Generic;
using Action = Pharmacy.Domain.Action;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Pharmacy.InfraStructure.GlobalVariables;
using DomainString = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public partial class ActionController : Controller
    {
        private readonly IActionService _actionSrv;
        private readonly IMemoryCacheProvider _cache;

        public ActionController(IActionService actionBiz, IMemoryCacheProvider cache)
        {
            _actionSrv = actionBiz;
            _cache = cache;
        }

        private List<SelectListItem> GetActions()
            => _actionSrv.Get(true).ToSelectListItems();

        [HttpGet]
        public virtual async Task<JsonResult> Add()
        {
            ViewBag.Actions = GetActions();
            return Json(new Modal
            {
                IsSuccessful = true,
                Title = $"{Strings.Add} {DomainString.Action}",
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Entity", new Action()),
                AutoSubmitUrl = Url.Action("Add", "Action"),
                AutoSubmit = false,
                ResetForm = true
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Add(Action model)
        {
            if (model.ParentId != null && (string.IsNullOrWhiteSpace(model.ControllerName) || string.IsNullOrWhiteSpace(model.ActionName)))
                return Json(new Response<string> { IsSuccessful = false, Message = Strings.ValidationFailed });

            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _actionSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            ViewBag.Actions = GetActions();
            var findRep = await _actionSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new Response<string> { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Action) });
            _cache.Remove(CacheSettings.MenuModelCacheKey(User.GetUserId()));
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Action}",
                AutoSubmitBtnText = Strings.Edit,
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Entity", findRep.Result),
                AutoSubmit = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Action model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            if (model.ParentId != null && (string.IsNullOrWhiteSpace(model.ControllerName) || string.IsNullOrWhiteSpace(model.ActionName)))
                return Json(new Response<string> { IsSuccessful = false, Message = Strings.ValidationFailed });

            return Json(await _actionSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _actionSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(ActionSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_actionSrv.Get(new ActionSearchFilter()));
            else return PartialView("Partials/_List", _actionSrv.Get(filter));
        }

        [HttpGet, AuthEqualTo("ActionInRole", "Add")]
        public virtual JsonResult Search(string q)
            => Json(_actionSrv.Search(q).ToSelectListItems());

    }
}