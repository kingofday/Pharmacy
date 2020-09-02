using Elk.Core;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;

namespace Pharmacy.API.Controllers
{
    [EnableCors("AllowedOrigins")]
    [ApiController, Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        readonly IDrugCategoryService _drugCategorySrv;
        public CategoryController(IDrugCategoryService drugCategorySrv)
        {
            _drugCategorySrv = drugCategorySrv;
        }

        [HttpGet]
        public ActionResult<IResponse<List<DrugCategoryDTO>>> GetAll()
        => _drugCategorySrv.Get();
    }
}