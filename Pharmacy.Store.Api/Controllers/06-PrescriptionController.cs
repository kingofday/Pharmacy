using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<ActionResult<IResponse<int>>> Add([FromServices] IWebHostEnvironment env, AddPrescriptionModel model)
        //{
        //    model.AppDir = env.WebRootPath;
        //    if(User.Identity.IsAuthenticated)
        //        model.UserId = User.GetUserId();
        //    return await _prescriptionSrv.Add(model);
        //}
         => new Response<int> { IsSuccessful = true, Result = 1 };
    }
}