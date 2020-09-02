using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;

namespace Pharmacy.API.Controllers
{
    [EnableCors("AllowedOrigins")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        readonly IDrugService _drugService;
        readonly List<DrugDTO> items;
        public DrugController(IDrugService DrugService)
        {
            _drugService = DrugService;
            items = new List<DrugDTO> {
                new DrugDTO
                {
                    DrugId =1,
                    NameFa = "پماد ضد حساسیت",
                    NameEn = "Mouster Againt Alergic",
                    ShortDescription="برای برطرف نمودن حساسیت های موضعی",
                    Count =3,
                    UniqueId ="ABc12F",
                    DiscountPrice =2000,
                    Price  = 40000,
                    UnitName = "جعبه",
                    ThumbnailImageUrl = "http://www.admin.darukade.com/Icon/360/16242816778202021612.jpg"
                },
                new DrugDTO
                {
                    DrugId =2,
                    NameFa = "کرم ضد چروک پوست",
                    NameEn = "Mouster Againt Head Skin",
                    ShortDescription="برای جلوگیری از چروکیدگی",
                    Count =5,
                    UniqueId ="ABc50F",
                    DiscountPrice =1000,
                    Price  = 30000,
                    UnitName = "جعبه",
                    ThumbnailImageUrl = "http://www.admin.darukade.com/Icon/360/1101217943202071612.jpg"
                },
                new DrugDTO
                {
                    DrugId =3,
                    NameFa = "پماد شفاف سازی",
                    NameEn = "Mouster Againt Head Skin",
                    ShortDescription="برای جلوگیری از چروکیدگی",
                    Count =5,
                    UniqueId ="ABc50F",
                    DiscountPrice =1500,
                    Price  = 34000,
                    UnitName = "جعبه",
                    ThumbnailImageUrl = "http://www.admin.darukade.com/Icon/360/1221717973202072012.jpg"
                },
                new DrugDTO
                {
                    DrugId =4,
                    NameFa = "کرم شفاف سازی",
                    NameEn = "Mouster Againt Head Skin",
                    ShortDescription="برای جلوگیری از چروکیدگی",
                    Count =5,
                    UniqueId ="ABc50F",
                    DiscountPrice =1500,
                    Price  = 34000,
                    UnitName = "جعبه",
                    ThumbnailImageUrl = "http://www.admin.darukade.com/Icon/360/15565817810202062912.jpg"

                },
                new DrugDTO
                {
                    DrugId =5,
                    NameFa = "پماد شفاف سازی",
                    NameEn = "Mouster Againt Head Skin",

                    ShortDescription="برای جلوگیری از چروکیدگی",
                    Count =4,
                    UniqueId ="ABc50F",
                    DiscountPrice =2500,
                    Price  = 39000,
                    UnitName = "جعبه",
                    ThumbnailImageUrl = "http://www.admin.darukade.com/Icon/360/1221717973202072012.jpg"

                },
                new DrugDTO
                {
                    DrugId =6,
                    NameFa = "کرم جوان سازی",
                    NameEn = "Mouster Againt Head Skin",
                    ShortDescription="برای جلوگیری از چروکیدگی",
                    Count =4,
                    UniqueId ="ABc50F",
                    DiscountPrice =2500,
                    Price  = 41000,
                    UnitName = "جعبه",
                    ThumbnailImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2019/09/0028694_0-300x300.png"

                }
            };
        }

        [HttpGet, Route("[controller]")]
        public ActionResult<IResponse<GetDrugsModel>> Get([FromQuery] DrugSearchFilter filter)
                       => _drugService.GetAsDto(filter);

        [HttpGet, Route("drug/{id:int}")]
        public ActionResult<IResponse<SingleDrugDTO>> GetSingle(int id)
            => _drugService.GetSingle(id);
    }
}