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
    [Route("[controller]")]
    public class DeliveryProviderController : ControllerBase
    {
        readonly IDeliveryProviderService _deliveryProviderSrv;
        public DeliveryProviderController(IDeliveryProviderService deliveryProviderSrv)
        {
            _deliveryProviderSrv = deliveryProviderSrv;
        }

        [HttpGet]
        public ActionResult<IResponse<List<DeliveryDTO>>> Get()
                => _deliveryProviderSrv.GetAllAsDTO();
    }
}