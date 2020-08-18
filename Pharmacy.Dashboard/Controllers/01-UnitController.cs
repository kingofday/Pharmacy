using Elk.Http;
using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using DomainString = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public partial class UnitController : Controller
    {
        private readonly IUnitService _UnitSrv;

        public UnitController(IUnitService UnitSrv)
        {
            _UnitSrv = UnitSrv;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.Unit}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new Unit()),
                AutoSubmitUrl = Url.Action("Add", "Unit")
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(Unit model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _UnitSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(int id)
        {
            var findRep = await _UnitSrv.FindAsync(id);
            if (!findRep.IsSuccessful) return Json(new { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.Unit) });

            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.Unit}",
                AutoSubmitBtnText = Strings.Edit,
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", findRep.Result),
                AutoSubmitUrl = Url.Action("Update", "Unit"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(Unit model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _UnitSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _UnitSrv.DeleteAsync(id));

        [HttpGet]
        public virtual ActionResult Manage(UnitSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_UnitSrv.Get(filter));
            else return PartialView("Partials/_List", _UnitSrv.Get(filter));
        }

    }
}