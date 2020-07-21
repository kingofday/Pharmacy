using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Elk.Core;

namespace Pharmacy.Store.Api.Controllers
{
    [ApiController]
    public class DrugController : ControllerBase
    {
        readonly IDrugService _drugService;
        public DrugController(IDrugService DrugService)
        {
            _drugService = DrugService;
        }

        [HttpGet,Route("[controller]")]
        public ActionResult<IResponse<List<DrugDTO>>> Search(string q)
            //=> _drugService.Get(q);
            => new Response<List<DrugDTO>>
            {
                IsSuccessful = true,
                Result = new List<DrugDTO> {
                new DrugDTO
                {
                    DrugId =1,
                    PriceId =1,
                    NameFa = "پماد ضد حساسیت",
                    NameEn = "Mouster Againt Alergic",
                    ShortDescription="برای برطرف نمودن حساسیت های موضعی",
                    Count =3,
                    UniqueId ="ABc12F",
                    DiscountPrice =2000,
                    Price  = 40000,
                    UnitName = "جعبه",
                    ThumbnailImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2019/09/Capture.png"
                },
                new DrugDTO
                {
                    DrugId =2,
                    PriceId =2,
                    NameFa = "کرم ضد چروک پوست",
                    NameEn = "Mouster Againt Head Skin",
                    ShortDescription="برای جلوگیری از چروکیدگی",
                    Count =5,
                    UniqueId ="ABc50F",
                    DiscountPrice =1000,
                    Price  = 30000,
                    UnitName = "جعبه",
                    ThumbnailImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2019/09/0034385_0-300x300.png"
                }
            }
            };

        [HttpGet, Route("drug/{id:int}")]
        public ActionResult<IResponse<SingleDrugDTO>> GetSingle(int id)
            //=> _drugService.GetSingle(id);
            => new Response<SingleDrugDTO>
            {
                Result = new SingleDrugDTO
                {
                    DrugId = 1,
                    NameFa = "پماد ضد حساسیت",
                    NameEn = "Mouster Againt Alergic",
                    UniqueId = "ABc12F",
                    Slides = new List<string>{
                        "https://pharma.gocodeit.me/wp-content/uploads/2019/09/Capture.png"
                    }
                }
            };
    }
}