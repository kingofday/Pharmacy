using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Pharmacy.API.Controllers
{
    [ApiController, EnableCors("AllowedOrigins"),  Route("[controller]")]
    public class AddressController : ControllerBase
    {
        readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public ActionResult<IResponse<List<AddressDTO>>> Get()
        {
            return new Response<List<AddressDTO>>
            {
                IsSuccessful = true,
                Result = new List<AddressDTO>
                {
                    new AddressDTO
                    {
                        Id = 1,
                        Fullname = "کامران زرینی",
                        MobileNumber = 9334188184,
                        Details = "میدان ونک، خیابن گاندی پلاک 36",
                        Lat = 35.757474,
                        Lng = 51.410109
                    },
                    new AddressDTO
                    {
                        Id = 2,
                        Fullname = "هومن زرینی",
                        MobileNumber = 9334188189,
                        Details = "میدان آرزانتین، گوچه بهار پلاک 16",
                        Lat = 35.737040,
                        Lng = 51.415576
                    }
                }
            };
        }
            //=> _addressService.Get(User.GetUserId());


        [HttpPost]
        public async Task<ActionResult<IResponse<int>>> Add(AddressDTO model)
            //=> await _addressService.AddAsync(User.GetUserId(),model);
            => new Response<int> { IsSuccessful = true, Result = 1 };

        [HttpDelete]
        public async Task<ActionResult<IResponse<bool>>> Delete(int id)
            //=> await _addressService.DeleteAsync(User.GetUserId(), id);
            => new Response<bool> { IsSuccessful = true, Result= true };
    }
}