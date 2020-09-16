using Elk.Core;
using Pharmacy.Service;
using Pharmacy.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Pharmacy.API.Controllers
{
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