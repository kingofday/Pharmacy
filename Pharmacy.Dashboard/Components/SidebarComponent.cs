using Elk.Core;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Pharmacy.Dashboard
{
    public class Sidebar : ViewComponent
    {
        private readonly IUserService _userSrv;
        private readonly IConfiguration _configuration;
        private const string UrlPrefixKey = "CustomSettings:UrlPrefix";

        public Sidebar(IUserService userSrv, IConfiguration configuration)
        {
            _userSrv = userSrv;
            _configuration = configuration;
        }

        public IViewComponentResult Invoke()
        {
            var rep = _userSrv.GetAvailableActions(HttpContext.User.GetUserId(), null, _configuration.GetValue<string>(UrlPrefixKey));
            return View(rep);
        }
    }
}
