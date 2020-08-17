using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Pharmacy.API.Resources;

namespace Pharmacy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowedOrigins")]
    public class PrescriptionController : ControllerBase
    {
        readonly IPrescriptionService _prescriptionSrv;
        public PrescriptionController(IPrescriptionService prescriptionSrv)
        {
            _prescriptionSrv = prescriptionSrv;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromServices] IWebHostEnvironment env, [FromForm]AddPrescriptionModel model)
        {
            model.AppDir = env.WebRootPath;
            if (User.Identity.IsAuthenticated)
                model.UserId = User.GetUserId();
            else if(string.IsNullOrEmpty(model.MobileNumber) || !ModelState.IsValid)
                return Ok(new { Status = 401, Message = Domain.Resource.ErrorMessage.InvalidMobileNumber });
            return Ok(await _prescriptionSrv.Add(model));
        }
        //=> new Response<int> { IsSuccessful = true, Result = 1 };
    }
}