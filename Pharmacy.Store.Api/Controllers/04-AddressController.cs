using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Pharmacy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [Authorize, HttpGet]
        public ActionResult<IResponse<List<AddressDTO>>> Get()
        => _addressService.Get(User.GetUserId());
    }
}