using System;
using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service.Resource;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public class PostService : DeliveryAgentService
    {
        public PostService()
        {
            Name = "NormalPost";
        }

        public override Task<IResponse<dynamic>> AddressInquiry(LocationDTO location)
        {
            throw new NotImplementedException();
        }

        public override async  Task<IResponse<PriceInquiryResult>> PriceInquiry(LocationDTO origin, LocationDTO destination, bool cashed, bool hasReturn)
        {
            try
            {
                var result = new PriceInquiryResult
                {
                    DeliveryType = "Post",
                    DeliveryType_Fa = "پست",

                    Price = 12000,
                    Final_Price = 12000,
                    Distance = "0",
                    Discount = 0,
                    Duration = "0",
                    Delay = 0,
                    Cashed = cashed,
                    Has_Return = hasReturn,
                    Price_With_Return = 15000,
                    Addresses = null
                };

                return new Response<PriceInquiryResult>
                {
                    IsSuccessful = true,
                    Result = result
                };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                return new Response<PriceInquiryResult> { Message = ServiceMessage.Error };
            }
        }

        public override async Task<IResponse<OrderResult>> RegisterOrder(DeliveryOrderLocationDTO origin, DeliveryOrderLocationDTO destination, bool cashed, bool hasReturn, string extraParams)
           => new Response<OrderResult> { IsSuccessful = true };
    }
}
