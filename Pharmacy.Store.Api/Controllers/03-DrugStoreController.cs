using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Elk.Core;

namespace Pharmacy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrugStoreController : ControllerBase
    {
        readonly IConfiguration _configuration;
        readonly IDrugStoreService _drugStore;

        public DrugStoreController(IConfiguration configuration, IDrugStoreService drugStore)
        {
            _configuration = configuration;
            _drugStore = drugStore;
        }

        [HttpGet]
        public ActionResult<IResponse<List<DrugStoreDTO>>> Get()
               => new Response<List<DrugStoreDTO>>
               {
                   IsSuccessful = true,
                   Result = //_drugStore.GetAsDTO()
                   new List<DrugStoreDTO>{
                    new DrugStoreDTO{
                        DrugStoreId =1,
                        Name = "داروخانه یک",
                        ImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2020/06/group_13592-100x100.png"
                    },
                    new DrugStoreDTO{
                        DrugStoreId =2,
                        Name = "داروخانه دو",
                        ImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2020/06/group_13593-100x100.png"
                    },
                    new DrugStoreDTO{
                        DrugStoreId =3,
                        Name = "داروخانه یک",
                        ImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2020/06/group_13592-100x100.png"
                    },
                    new DrugStoreDTO{
                        DrugStoreId =4,
                        Name = "داروخانه دو",
                        ImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2020/06/group_13593-100x100.png"
                    },
                    new DrugStoreDTO{
                        DrugStoreId =5,
                        Name = "داروخانه یک",
                        ImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2020/06/group_13592-100x100.png"
                    },
                    new DrugStoreDTO{
                        DrugStoreId =6,
                        Name = "داروخانه دو",
                        ImageUrl = "https://pharma.gocodeit.me/wp-content/uploads/2020/06/group_13593-100x100.png"
                    }
                    }
               };


    }
}