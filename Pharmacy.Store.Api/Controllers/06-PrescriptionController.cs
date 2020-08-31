using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Pharmacy.API.Resources;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Text;
using System.IO;

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
        public async Task<ActionResult> Add([FromServices] IOptions<APICustomSetting> settings, [FromForm] AddPrescriptionModel model)
        {
            if (User.Identity.IsAuthenticated)
                model.UserId = User.GetUserId();
            else if (string.IsNullOrEmpty(model.MobileNumber) || !ModelState.IsValid)
                return Ok(new { Status = 401, Message = Domain.Resource.ErrorMessage.InvalidMobileNumber });
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.MobileNumber, Encoding.UTF8, "text/plain"), nameof(model.MobileNumber));
            foreach (var file in model.Files)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var byteArrayContent = new ByteArrayContent(ms.ToArray());
                byteArrayContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { FileName = file.FileName, Name = "\"files\"" };
                content.Add(byteArrayContent, "Files");
            }
            using var http = new HttpClient();
            var call = await http.PostAsync(settings.Value.DashboardAddPrescriptionUrl, content);
            if (call.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var rep = await call.Content.ReadAsStringAsync();
                return Ok(rep.DeSerializeJson<Response<int>>());
            }
            return Ok(new Response<int> { Message = Strings.Error });
        }

    }
}