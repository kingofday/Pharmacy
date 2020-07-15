using System;
using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Pharmacy.Store.Api.Resources;
using Microsoft.Extensions.Configuration;

namespace Pharmacy.Store.Api.Controllers
{
    public class AddressController : Controller
    {
        readonly IConfiguration _configuration;
        readonly IAddressService _addressService;
        readonly IDeliveryService _deliverySrv;
        public AddressController(IAddressService addressService, IDeliveryService deliverySrv, IConfiguration configuration)
        {
            _addressService = addressService;
            _deliverySrv = deliverySrv;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (!Guid.TryParse(Request.Headers["token"], out Guid userId))
                return Json(new Response<List<AddressDTO>>
                {
                    IsSuccessful = false,
                    Message = Strings.ThereIsNoToken
                });
            return Json(_addressService.Get(userId));
        }

        [HttpGet]
        public async Task<IActionResult> GetDeliveryCost(int storeId, LocationDTO location)
                => Json(await _deliverySrv.GetDeliveryTypes(storeId, location));
    }
}