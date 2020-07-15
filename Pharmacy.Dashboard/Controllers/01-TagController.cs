using Elk.Http;
using Pharmacy.Service;
using Pharmacy.Domain;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using DomainString = Pharmacy.Domain.Resource.Strings;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Pharmacy.Dashboard.Controllers
{

    public class TagController : Controller
    {
        private readonly ITagService _tagSrv;
        public TagController(ITagService tagService)
        {
            _tagSrv = tagService;
        }

        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Tag}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Tag()),
                AutoSubmitUrl = Url.Action("Add", "Tag")
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(Tag model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _tagSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _tagSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Tag}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "Tag"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Tag model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _tagSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _tagSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(TagSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_tagSrv.Get(filter));
            else return PartialView("Partials/_List", _tagSrv.Get(filter));
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Search(string q)
        {
            var rep = _tagSrv.Get(new TagSearchFilter { TitleF = q });
            if (rep.Items.Any())
                return Json(rep.Items.Select(x => new SelectListItem
                {
                    Value = x.TagId.ToString(),
                    Text = x.Title
                }));
            else return Json(new List<SelectListItem>());
        }
    }
}