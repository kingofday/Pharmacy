using Elk.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Pharmacy.API.Controllers
{
    public class HomeController : ControllerBase
    {
        [HttpGet,Route("/")]
        public ActionResult<Response<bool>> Index() => new Response<bool> { IsSuccessful = true, Message = "Welcome to Pharma Api" };

        //[HttpGet, Route("contactus")]
        //public ActionResult ContactUs()
        //{
        //    return new Response
        //    {
        //        IsSuccessful = true,
        //        Result = new
        //        {
        //            WhatsappLink = "https://wa.me/989116107197",
        //            TelegramLink = "https://t.me/kingofday",
        //            PhoneNumbers = new List<string> { "9334188188", "933561109" },
        //            WebsiteName = "Pharmacy.ir",
        //            WebsiteUrl = "https://kingofday.ir"
        //        }
        //    });
        //}
    }
}
