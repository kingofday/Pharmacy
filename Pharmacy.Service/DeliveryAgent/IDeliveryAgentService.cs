using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public abstract class DeliveryAgentService
    {
        public string Name { get; set; }
        public abstract Task<IResponse<dynamic>> AddressInquiry(LocationDTO location);
        public abstract Task<IResponse<PriceInquiryResult>> PriceInquiry(LocationDTO origin, LocationDTO destination, bool cashed, bool hasReturn);
        public abstract Task<IResponse<OrderResult>> RegisterOrder(DeliveryOrderLocationDTO origin, DeliveryOrderLocationDTO destination, bool cashed, bool hasReturn, string extraParams);
    }
}
