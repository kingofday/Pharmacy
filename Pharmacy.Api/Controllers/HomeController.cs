using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Wellcome To Pharmacy Api...");
        }

    }
}
