using System;
using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Dashboard.Resources;
using DomainString = Pharmacy.Domain.Resource.Strings;
using Elk.Http;

namespace Pharmacy.Dashboard.Controllers
{
    [AuthorizationFilter]
    public partial class UserController : Controller
    {
        private readonly IUserService _userSrv;

        public UserController(IUserService userBiz)
        {
            _userSrv = userBiz;
        }


        [HttpGet]
        public virtual JsonResult Add()
            => Json(new Modal
            {
                Title = $"{Strings.Add} {DomainString.User}",
                Body = ControllerExtension.RenderViewToString(this, "Partials/_Entity", new User()),
                AutoSubmitUrl = Url.Action("Add", "User")
            });

        [HttpPost]
        public virtual async Task<JsonResult> Add(User model)
        {
            ModelState.Remove(nameof(model.LastLoginDateSh));
            ModelState.Remove(nameof(model.InsertDateSh));
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _userSrv.AddAsync(model));
        }

        [HttpGet]
        public virtual async Task<JsonResult> Update(Guid id)
        {
            var findUser = await _userSrv.FindAsync(id);
            if (!findUser.IsSuccessful) return Json(new Response<string> { IsSuccessful = false, Message = Strings.RecordNotFound.Fill(DomainString.User) });
            return Json(new Modal
            {
                Title = $"{Strings.Update} {DomainString.User}",
                AutoSubmitBtnText = Strings.Edit,
                Body = await ControllerExtension.RenderViewToStringAsync(this, "Partials/_Entity", findUser.Result),
                AutoSubmitUrl = Url.Action("Update", "User"),
                ResetForm = false
            });
        }

        [HttpPost]
        public virtual async Task<JsonResult> Update(User model)
        {
            ModelState.Remove(nameof(model.LastLoginDateSh));
            ModelState.Remove(nameof(model.InsertDateSh));
            ModelState.Remove(nameof(model.Password));
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            return Json(await _userSrv.UpdateAsync(model));
        }

        [HttpPost]
        public virtual async Task<JsonResult> Delete(Guid id) => Json(await _userSrv.DeleteAsync(id));

        [HttpGet]
        public virtual async Task<ActionResult> ProfileInfo()
        {
            var user = await _userSrv.FindAsync(User.GetUserId());
            return View(new UpdateProfileModel().CopyFrom(user.Result));
        }

        [HttpPost]
        public virtual async Task<JsonResult> ProfileInfo(UpdateProfileModel model)
        {
            var id = User.GetUserId();
            if (id != model.UserId) return Json(new Response<string> { IsSuccessful = false, Message = Strings.Error });
            return Json(await _userSrv.UpdateProfile(id, model));
        }

        [HttpGet]
        public virtual ActionResult Manage(UserSearchFilter filter)
        {
            if (!Request.IsAjaxRequest()) return View(_userSrv.Get(filter));
            else return PartialView("Partials/_List", _userSrv.Get(filter));
        }

        [HttpGet, AuthEqualTo("UserInRole", "Add")]
        public virtual JsonResult Search(string q)
            => Json(_userSrv.Search(q).ToSelectListItems());

    }
}