using Elk.Core;
using System.IO;
using System.Text;
using System.Linq;
using System.Net.Http;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;
using Pharmacy.API.Resources;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Pharmacy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrescriptionController : ControllerBase
    {
        readonly IPrescriptionService _prescriptionSrv;
        readonly APICustomSetting _settings;
        public PrescriptionController(IPrescriptionService prescriptionSrv, IOptions<APICustomSetting> settings)
        {
            _prescriptionSrv = prescriptionSrv;
            _settings = settings.Value;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm] AddPrescriptionModel model)
        {
            if (User.Identity.IsAuthenticated)
                model.UserId = User.GetUserId();
            else if (string.IsNullOrEmpty(model.MobileNumber) || !ModelState.IsValid)
                return Ok(new { Status = 401, Message = Domain.Resource.ErrorMessage.InvalidMobileNumber });

            var content = new MultipartFormDataContent();
            if (User.Identity.IsAuthenticated)
            {
                content.Add(new StringContent(model.UserId.ToString(), Encoding.UTF8, "text/plain"), nameof(model.UserId));
                content.Add(new StringContent(User.Claims.First(x=>x.Type == ClaimTypes.MobilePhone).Value, Encoding.UTF8, "text/plain"), nameof(model.MobileNumber));
            }
            else
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
            var call = await http.PostAsync(_settings.DashboardAddPrescriptionUrl, content);
            if (call.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var rep = await call.Content.ReadAsStringAsync();
                return Ok(rep.DeSerializeJson<Response<int>>());
            }
            return Ok(new Response<int> { Message = Strings.Error });
        }

        [HttpGet]
        public ActionResult<IResponse<List<DrugDTO>>> GetItems(int id) => _prescriptionSrv.GetItems(id, _settings.DashboardBaseUrl);
    }
}