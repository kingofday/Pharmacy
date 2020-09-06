using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Options;

namespace Pharmacy.API.Controllers
{
    [EnableCors("AllowedOrigins")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        readonly IDrugService _drugService;
        readonly APICustomSetting _settings;
        public DrugController(IDrugService DrugService, IOptions<APICustomSetting> settings)
        {
            _drugService = DrugService;
            _settings = settings.Value;
        }

        [HttpGet, Route("[controller]")]
        public ActionResult<IResponse<GetDrugsModel>> Get([FromQuery] DrugSearchFilter filter)
                       => _drugService.GetAsDto(filter, _settings.DashboardBaseUrl);

        [HttpGet, Route("drug/{id:int}")]
        public ActionResult<IResponse<SingleDrugDTO>> GetSingle(int id)
            => _drugService.GetSingle(id, _settings.DashboardBaseUrl);
    }
}