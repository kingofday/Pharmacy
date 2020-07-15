using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Delivery.Service
{
    public static class PostProvider
    {
        public static async Task<PriceInquiryResult> PriceInquiry(LocationDTO originLocation, LocationDTO destinationLocation, bool cashed, bool hasReturn)
        {
            try
            {
                var result = new PriceInquiryResult
                {
                    DeliveryProviderId = 2,
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

                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                return null;
            }
        }


    }
}
