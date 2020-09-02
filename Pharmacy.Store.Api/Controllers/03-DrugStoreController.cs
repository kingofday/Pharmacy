using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Elk.Core;
using Microsoft.Extensions.Options;

namespace Pharmacy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrugStoreController : ControllerBase
    {
        readonly IConfiguration _configuration;
        readonly IDrugStoreService _drugStoreSrv;

        public DrugStoreController(IConfiguration configuration, IDrugStoreService drugStore)
        {
            _configuration = configuration;
            _drugStoreSrv = drugStore;
        }

        [HttpGet]
        public ActionResult<IResponse<List<DrugStoreDTO>>> Get([FromServices]IOptions<APICustomSetting> settings)
            => new Response<List<DrugStoreDTO>> { IsSuccessful = true, Result = _drugStoreSrv.GetAsDTO(settings.Value.DashboardBaseUrl) };


    }
}