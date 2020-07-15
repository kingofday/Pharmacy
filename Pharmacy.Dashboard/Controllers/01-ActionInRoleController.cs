using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using Microsoft.AspNetCore.Authorization;
using DomainString = Pharmacy.Domain.Resource.Strings;

namespace Pharmacy.Dashboard.Controllers
{

    [AuthorizationFilter]
    public partial class ActionInRoleController : Controller
    {
        readonly IActionInRoleService _actionInRoleSrv;

        public ActionInRoleController(IActionInRoleService actionInRoleSrv)
        {
            _actionInRoleSrv = actionInRoleSrv;
        }

        [HttpGet, AllowAnonymous]
        public virtual async Task<JsonResult> Add(int id)
            => Json(new Modal
            {
                AutoSubmit = false,
                Title = $"{Strings.Add} {DomainString.Action}",
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Entity", new ActionInRole { RoleId = id }),
                AutoSubmitUrl = Url.Action("Add", "ActionInRole"),

            });

        [HttpPost, AllowAnonymous]
        public virtual async Task<JsonResult> Add(ActionInRole model)
        {
            if (!ModelState.IsValid) return Json(new { IsSuccessful = false, Message = ModelState.GetModelError() });
            var addRep = await _actionInRoleSrv.AddAsync(model);

            if (!addRep.IsSuccessful) return Json(addRep);
            var getRep = _actionInRoleSrv.GetRoles(model.ActionId).ToList();
            getRep.ForEach((x) =>
            {
                x.Role.ActionInRoles = null;
            });

            return Json(new Response<string>
            {
                IsSuccessful = true,
                Result = ControllerExtension.RenderViewToString(this, "Partials/_ListViaAction", getRep)
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(int id) => Json(await _actionInRoleSrv.DeleteAsync(id));

        [HttpGet, AuthEqualTo("ActionInRole", "Add")]
        public virtual PartialViewResult GetByAction(int actionId) => PartialView("Partials/_ListViaAction", _actionInRoleSrv.GetRoles(actionId));

        [HttpGet, AuthEqualTo("ActionInRole", "Add")]
        public virtual async Task<JsonResult> GetByRole(int roleId)
            => Json(new Modal
            {
                IsSuccessful = true,
                Title = $"{DomainString.Action}",
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_ListViaRole", _actionInRoleSrv.GetActions(roleId)),
                AutoSubmitUrl = Url.Action("Add", "Action"),
                AutoSubmit = false
            });
    }
}