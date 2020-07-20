using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Elk.Core;

namespace Pharmacy.Store.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrugController : ControllerBase
    {
        readonly IDrugService _drugService;
        public DrugController(IDrugService DrugService)
        {
            _drugService = DrugService;
        }

        [HttpGet]
        public ActionResult<IResponse<List<DrugDTO>>> Search(string q)
            //=> _drugService.Get(q);
            => new Response<List<DrugDTO>>
            {
                Result = new List<DrugDTO> {
                new DrugDTO
                {
                    DrugId =1,
                    PriceId =1,
                    NameFa = "پماد ضد حساسیت",
                    NameEn = "Mouster Againt Alergic",
                    Count =3,
                    UniqueId ="ABc12F",
                    DiscountPrice =2000,
                    Price  = 40000,
                    UnitName = "جعبه",
                    TumbnailImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2019/09/Capture.png"
                },
                new DrugDTO
                {
                    DrugId =1,
                    PriceId =1,
                    NameFa = "کرم ضد چروک پوست",
                    NameEn = "Mouster Againt Head Skin",
                    Count =5,
                    UniqueId ="ABc12F",
                    DiscountPrice =1000,
                    Price  = 30000,
                    UnitName = "جعبه",
                    TumbnailImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2019/09/0034385_0-300x300.png"
                }
            }
            };

        [HttpGet]
        public ActionResult<IResponse<SingleDrugDTO>> GetSingle(int id) => _drugService.GetSingle(id);
    }
}