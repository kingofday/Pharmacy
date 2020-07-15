using Pharmacy.Domain;
using System.Threading.Tasks;
using Pharmacy.Delivery.Service;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Delivery.Controllers
{
    public class OrderController : Controller
    {
        public IDeliveryService _deliveryService { get; }

        public OrderController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }


        [HttpPost]
        public async Task<IActionResult> Peyk([FromBody]DeliveryOrderDTO deliveryOrderDTO)
            => Ok(await _deliveryService.RegisterPeykOrder(deliveryOrderDTO));


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DeliveryOrderDTO deliveryOrderDTO)
            => Ok(await _deliveryService.RegisterPostOrder(deliveryOrderDTO));
    }
}