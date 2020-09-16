using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;

namespace Pharmacy.API.Controllers
{
    [ApiController, Route("[controller]"), CustomAuth]
    public class AddressController : ControllerBase
    {
        readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public ActionResult<IResponse<List<AddressDTO>>> Get()
                => _addressService.Get(User.GetUserId());


        [HttpPost]
        public async Task<ActionResult<IResponse<int>>> Add(AddressDTO model)
            => await _addressService.AddAsync(User.GetUserId(),model);

        [HttpPut]
        public async Task<ActionResult<IResponse<int>>> Update(AddressDTO model)
            => await _addressService.UpdateAsync(User.GetUserId(),model);

        [HttpDelete]
        public async Task<ActionResult<IResponse<bool>>> Delete(int id)
            => await _addressService.DeleteAsync(User.GetUserId(), id);
    }
}