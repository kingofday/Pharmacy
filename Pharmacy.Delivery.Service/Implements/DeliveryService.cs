using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pharmacy.Delivery.Service.Resource;

namespace Pharmacy.Delivery.Service
{
    public class DeliveryService : IDeliveryService
    {
        public async Task<IResponse<dynamic>> AddressInquiry(LocationDTO location)
            => await AloPeikProvider.AddressInquiry(location);

        public async Task<IResponse<List<PriceInquiryResult>>> PriceInquiry(LocationsDTO priceInquiry, bool cashed, bool hasReturn)
        {
            var result = new Response<List<PriceInquiryResult>>();
            result.Result = new List<PriceInquiryResult>();
            try
            {
                var peyk = await AloPeikProvider.PriceInquiry(priceInquiry.Source, priceInquiry.Destination, cashed, hasReturn);
                var post = await PostProvider.PriceInquiry(priceInquiry.Source, priceInquiry.Destination, cashed, hasReturn);

                if (peyk.IsNotNull()) result.Result.Add(peyk);
                if (post.IsNotNull()) result.Result.Add(post);
                result.IsSuccessful = true;
                result.Message = ServiceMessage.Success;
                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                result.Message = ServiceMessage.Exception;
                return result;
            }
        }

        public async Task<IResponse<OrderResult>> RegisterPeykOrder(DeliveryOrderDTO deliveryOrderDTO)
        {
            var result = new Response<OrderResult>();
            try
            {
                var registerOrderResult = await AloPeikProvider.RegisterOrder(deliveryOrderDTO.Addresses.FirstOrDefault(x=> x.Type == "origin"), deliveryOrderDTO.Addresses.FirstOrDefault(x => x.Type == "destination"), false, false, deliveryOrderDTO.ExtraParams);
                if (registerOrderResult == null) return new Response<OrderResult>() { Message = ServiceMessage.Error };

                result.Result = new OrderResult
                {
                    OrderId = registerOrderResult.Id,
                    OrderToken = registerOrderResult.Order_Token,
                    OrderDiscount = registerOrderResult.Order_Discount,
                    Price = registerOrderResult.Final_Price,
                    //ExtraParams = registerOrderResult.Extra_Param,
                    PayAtDestination = registerOrderResult.Pay_At_Dest,
                    Cashed = registerOrderResult.Cashed,
                    Delay = registerOrderResult.Delay,
                    Duration = registerOrderResult.Duration,
                    Distance = registerOrderResult.Distance,
                    Has_Return = registerOrderResult.Has_Return,
                    Addresses = registerOrderResult.Addresses
                };
                result.IsSuccessful = true;
                result.Message = ServiceMessage.Success;
                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                result.Message = ServiceMessage.Exception;
                return result;
            }
        }

        public async Task<IResponse<OrderResult>> RegisterPostOrder(DeliveryOrderDTO deliveryOrderDTO)
        {
            var result = new Response<OrderResult>();
            try
            {
                var registerOrderResult = await AloPeikProvider.RegisterOrder(deliveryOrderDTO.Addresses.FirstOrDefault(x => x.Type == "origin"), deliveryOrderDTO.Addresses.FirstOrDefault(x => x.Type == "destination"), false, false, deliveryOrderDTO.ExtraParams);

                result.Result = new OrderResult
                {
                    OrderId = registerOrderResult.Id,
                    OrderToken = registerOrderResult.Order_Token,
                    OrderDiscount = registerOrderResult.Order_Discount,
                    Price = registerOrderResult.Final_Price,
                    //ExtraParams = registerOrderResult.Extra_Param,
                    PayAtDestination = registerOrderResult.Pay_At_Dest,
                    Cashed = registerOrderResult.Cashed,
                    Delay = registerOrderResult.Delay,
                    Duration = registerOrderResult.Duration,
                    Distance = registerOrderResult.Distance,
                    Has_Return = registerOrderResult.Has_Return,
                    Addresses = registerOrderResult.Addresses
                };
                result.IsSuccessful = true;
                result.Message = ServiceMessage.Success;
                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                result.Message = ServiceMessage.Exception;
                return result;
            }
        }
    }
}
