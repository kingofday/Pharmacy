using Elk.Core;
using Elk.AspNetCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Pharmacy.Domain;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pharmacy.Service;
using Pharmacy.Dashboard.Resources;
using Microsoft.AspNetCore.Http;

namespace Pharmacy.Dashboard.Controllers
{
    public class StorePublicController : AuthBaseController
    {
        readonly IStoreService _storeSrv;
        readonly IConfiguration _configuration;
        readonly IUserService _userSrv;
        public StorePublicController(IConfiguration configuration, IStoreService storeSrv, IHttpContextAccessor httpAccessor, IUserService userSrv) : base(httpAccessor)
        {
            _configuration = configuration;
            _storeSrv = storeSrv;
            _userSrv = userSrv;
        }

        [HttpGet]
        public async Task<IActionResult> ShowPage(StoreType storeType, string username)
        {
            using var http = new HttpClient();
            var apiCall = await http.GetAsync($"{_configuration["CustomSettings:Crawler:GetPage"]}?username={username}");
            if (!apiCall.IsSuccessStatusCode) return Json(new Response<string> { Message = Strings.Error });
            var response = await apiCall.Content.ReadAsStringAsync();
            var crawl = response.DeSerializeJson<Response<CrawledPageDto>>();
            return Json(new Response<string>
            {
                IsSuccessful = crawl.IsSuccessful,
                Message = crawl.Message,
                Result = ControllerExtension.RenderViewToString(this, "Partials/_Instagram", crawl.Result),
            });
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            ViewBag.Types = EnumExtension.GetEnumElements<StoreType>().Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = x.Description
            }).ToList();
            return View(new StoreSignUpModel());
        }


        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([FromBody]StoreSignUpModel model)
        {
            if (!ModelState.IsValid) return Json(new Response<string> { IsSuccessful = false, Message = ModelState.GetModelError() });
            using var pageInfoHttp = new HttpClient();
            var callGetPageInfo = await pageInfoHttp.GetAsync($"{_configuration["CustomSettings:Crawler:GetPage"]}?username={model.Username}");
            if (!callGetPageInfo.IsSuccessStatusCode) return Json(new Response<string> { Message = Strings.Error });
            var crawl = (await callGetPageInfo.Content.ReadAsStringAsync()).DeSerializeJson<Response<CrawledPageDto>>();
            using var postsHttp = new HttpClient();
            var callAddPosts = await postsHttp.PostAsync($"{_configuration["CustomSettings:Crawler:AddPosts"]}?username={model.Username}", null);
            if (!callAddPosts.IsSuccessStatusCode) return Json(new Response<string> { Message = Strings.Error });
            var addPosts = (await callAddPosts.Content.ReadAsStringAsync()).DeSerializeJson<Response<bool>>();
            if (!addPosts.IsSuccessful) return Json(new Response<string> { Message = addPosts.Message });
            model.StoreRoleId = int.Parse(_configuration["CustomSettings:StoreRoleId"]);
            var signup = await _storeSrv.SignUp(model);
            if (!signup.IsSuccessful) return Json(new Response<string> { IsSuccessful = signup.IsSuccessful, Message = signup.Message });
            var menuRep = _userSrv.GetAvailableActions(signup.Result.UserId, null, _configuration["CustomSettings:UrlPrefix"]);
            if (menuRep == null) return Json(new Response<string> { IsSuccessful = false, Message = Strings.ThereIsNoViewForUser });
            await CreateCookie(signup.Result.User, false);
            return Json(new Response<string>
            {
                Result = Url.Action(menuRep.DefaultUserAction.Action, menuRep.DefaultUserAction.Controller, new { }),
                IsSuccessful = signup.IsSuccessful,
                Message = signup.Message
            });
        }
    }
}