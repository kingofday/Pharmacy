using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Store.Api.Controllers
{
    public class DrugController : Controller
    {
        readonly IDrugService _drugService;
        public DrugController(IDrugService DrugService)
        {
            _drugService = DrugService;
        }

        [HttpGet]
        public JsonResult Get(DrugSearchFilter filter) => Json(_drugService.GetAsDto(filter));

        [HttpGet]
        public JsonResult GetSingle(int id) => Json(_drugService.GetSingle(id));
    }
}