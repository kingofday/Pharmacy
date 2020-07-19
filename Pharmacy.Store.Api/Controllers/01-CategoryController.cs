using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Domain;
using System.Collections.Generic;
using Elk.Core;

namespace Pharmacy.Store.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        readonly IDrugCategoryService _drugCategorySrv;
        public CategoryController(IDrugCategoryService drugCategorySrv)
        {
            _drugCategorySrv = drugCategorySrv;
        }

        [HttpGet]
        public ActionResult<IResponse<List<DrugCategoryDTO>>> GetAll()
        //=> _drugCategorySrv.Get();
        => new Response<List<DrugCategoryDTO>>
        {
            IsSuccessful = true,
            Result = new List<DrugCategoryDTO>
                        {
                            new DrugCategoryDTO{
                                CategoryId = 1,
                                Name = "آرایشی و بهداشتی"
                            },
                            new DrugCategoryDTO{
                                CategoryId = 2,
                                Name = "تغذیه و سبک زندگی"
                            }
                        }
        };
    }
}