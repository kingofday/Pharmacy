using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Pharmacy.Store.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => Json(new {IsSuccessful = true,Message="Welcome to Pharmacy Store Api" });

        public IActionResult ContactUs()
        {
            return Json(new
            {
                IsSuccessful = true,
                Result = new
                {
                    WhatsappLink = "https://wa.me/989116107197",
                    TelegramLink = "https://t.me/kingofday",
                    PhoneNumbers = new List<string> { "9334188188", "933561109" },
                    WebsiteName = "Pharmacy.ir",
                    WebsiteUrl = "https://kingofday.ir"
                }
            });
        }
    }
}
