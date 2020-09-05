using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using System;

namespace Pharmacy.API.Controllers
{
    [EnableCors("AllowedOrigins")]
    [ApiController]
    [Route("[controller]")]
    public class DeliveryProviderController : ControllerBase
    {
        readonly IDeliveryProviderService _deliveryProviderSrv;
        readonly IOrderService _orderSrv;
        public DeliveryProviderController(IDeliveryProviderService deliveryProviderSrv, IOrderService orderSrv)
        {
            _deliveryProviderSrv = deliveryProviderSrv;
            _orderSrv = orderSrv;
        }

        [HttpGet]
        public ActionResult<IResponse<List<DeliveryDTO>>> Get() => _deliveryProviderSrv.GetAllAsDTO();

        [HttpGet, Route("{id:Guid}")]
        public ActionResult<IResponse<GetDeliveryPriceDTO>> GetPrice(Guid id) => _orderSrv.GetDeliveryPrice(id);
    }
}