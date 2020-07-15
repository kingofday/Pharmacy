using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Delivery.Service
{
    public interface IDeliveryService
    {
        Task<IResponse<dynamic>> AddressInquiry(LocationDTO location);
        Task<IResponse<List<PriceInquiryResult>>> PriceInquiry(LocationsDTO priceInquiry, bool cashed, bool hasReturn);
        Task<IResponse<OrderResult>> RegisterPeykOrder(DeliveryOrderDTO deliveryOrderDTO);
        Task<IResponse<OrderResult>> RegisterPostOrder(DeliveryOrderDTO deliveryOrderDTO);
    }
}
