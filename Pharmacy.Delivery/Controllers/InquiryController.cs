using Pharmacy.Domain;
using System.Threading.Tasks;
using Pharmacy.Delivery.Service;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.Delivery.Controllers
{
    public class InquiryController : Controller
    {
        public IDeliveryService _deliveryService { get; }

        public InquiryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }


        [HttpGet]
        public IActionResult Index()
            => Ok("WellCome To Pharmacy.Delivery Api ...");


        [HttpGet]
        public async Task<IActionResult> Address(LocationDTO location)
            => Ok(await _deliveryService.AddressInquiry(location));


        [HttpPost]
        public async Task<IActionResult> Price([FromBody]LocationsDTO priceInquiry)
            => Ok(await _deliveryService.PriceInquiry(priceInquiry, false, false));
        
    }
}