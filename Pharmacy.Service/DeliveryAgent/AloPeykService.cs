using Elk.Core;
using Elk.Http;
using Pharmacy.Domain;
using Pharmacy.InfraStructure;
using Pharmacy.Service.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public class AloPeykService : DeliveryAgentService
    {
        public AloPeykService()
        {
            Name = "AloPeyk";
        }
        public async Task<AloPeikUser> Authenticate()
        {
            var authResult = await HttpRequestTools.GetAsync<AloPeikResult<AloPeikUser>>(GlobalVariables.DeliveryProviders.AloPeik.Url, Encoding.UTF8);
            if (authResult.Status == "success") return authResult.Object;

            return null;
        }

        public override async Task<IResponse<PriceInquiryResult>> PriceInquiry(LocationDTO origin, LocationDTO destination, bool cashed, bool hasReturn)
        {
            var result = new PriceInquiryResult();
            try
            {
                #region Create Request Bode
                var model = new
                {
                    transport_type = AloPeikTransportType.motor_taxi.ToString(),
                    addresses = new List<dynamic> {
                        new { type = AloPeikAddressType.origin.ToString(), lat =  origin.Latitude, lng = origin.Longitude},
                        new { type = AloPeikAddressType.destination.ToString(), lat =  destination.Latitude, lng = destination.Longitude}
                        },
                    has_return = hasReturn,
                    cashed = cashed
                };
                #endregion

                using (var httpClient = new HttpClient())
                {
                    var body = new StringContent(model.SerializeToJson(), Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariables.DeliveryProviders.AloPeik.Token);
                    var response = await httpClient.PostAsync($"{GlobalVariables.DeliveryProviders.AloPeik.Url}/orders/price/calc", body);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseResult = responseBody.DeSerializeJson<AloPeikResult<PriceInquiryResult>>();
                    if (responseResult.Status == "success")
                    {
                        result = new PriceInquiryResult
                        {
                            DeliveryType = "Peyk",
                            DeliveryType_Fa = "پیک",

                            Price = responseResult.Object.Price,
                            Final_Price = responseResult.Object.Final_Price,
                            Distance = responseResult.Object.Distance,
                            Discount = responseResult.Object.Discount,
                            Duration = responseResult.Object.Duration,
                            Delay = responseResult.Object.Delay,
                            Cashed = responseResult.Object.Cashed,
                            Has_Return = responseResult.Object.Has_Return,
                            Price_With_Return = responseResult.Object.Price_With_Return,
                            Addresses = new List<AloPeikAddress> {
                                new AloPeikAddress{ Type = responseResult.Object.Addresses[0].Type, Address = responseResult.Object.Addresses[0].Address, City_Fa = responseResult.Object.Addresses[0].City_Fa},
                                new AloPeikAddress{ Type = responseResult.Object.Addresses[1].Type, Address = responseResult.Object.Addresses[1].Address, City_Fa = responseResult.Object.Addresses[1].City_Fa},
                            }
                        };
                    }
                    else return new Response<PriceInquiryResult> { Message = ServiceMessage.Error };
                }

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
        public override async Task<IResponse<dynamic>> AddressInquiry(LocationDTO location)
        {
            var result = new Response<dynamic>();
            try
            {
                var responseBody = string.Empty;
                using (var webClient = new HttpClient())
                {
                    webClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariables.DeliveryProviders.AloPeik.Token);
                    responseBody = await webClient.GetStringAsync($"{GlobalVariables.DeliveryProviders.AloPeik.Url}/locations?latlng={location.Latitude},{location.Longitude}");

                    var response = responseBody.DeSerializeJson<AloPeikResult<AloPeikAddressInquiry>>();
                    if (response.Status == "success")
                    {
                        result.Result = new
                        {
                            Address = response.Object.Address.First(),
                            Province = response.Object.Province,
                            District = response.Object.District,
                            City_fa = response.Object.City_fa
                        };
                        result.IsSuccessful = true;
                    }
                    else
                        result.Message = ServiceMessage.Error;
                }

                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                result.Message = ServiceMessage.Error;
                return result;
            }
        }
        public override async Task<IResponse<OrderResult>> RegisterOrder(DeliveryOrderLocationDTO origin, DeliveryOrderLocationDTO destination, bool cashed, bool hasReturn, string extraParams)
        {
            var result = new AloPeikOrderResult();
            try
            {
                #region Create Request Bode
                var model = new
                {
                    transport_type = AloPeikTransportType.motor_taxi.ToString(),
                    addresses = new List<dynamic> {
                        new { type = AloPeikAddressType.origin.ToString(), lat =  origin.Lat, lng = origin.Lng, description = origin.Description, person_phone = origin.PersonPhone, person_fullname = origin.PersonFullName},
                        new { type = AloPeikAddressType.destination.ToString(), lat =  destination.Lat, lng = destination.Lng, description = destination.Description, person_phone = destination.PersonPhone, person_fullname = destination.PersonFullName}
                        },
                    has_return = hasReturn,
                    cashed = cashed,
                    extra_params = extraParams
                };
                #endregion

                using (var httpClient = new HttpClient())
                {
                    var body = new StringContent(model.SerializeToJson(), Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariables.DeliveryProviders.AloPeik.Token);
                    var response = await httpClient.PostAsync($"{GlobalVariables.DeliveryProviders.AloPeik.Url}/orders", body);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseResult = responseBody.DeSerializeJson<AloPeikResult<AloPeikOrderResult>>();
                    if (responseResult.Status == "success")
                        result = responseResult.Object;
                    else
                        result = null;
                }

                return new Response<OrderResult> { IsSuccessful = true, Result = new OrderResult {
                    OrderToken = result.Order_Token,
                    OrderDiscount = result.Order_Discount,
                    Price = result.Final_Price,
                    //ExtraParams = registerOrderResult.Extra_Param,
                    PayAtDestination = result.Pay_At_Dest,
                    Cashed = result.Cashed,
                    Delay = result.Delay,
                    Duration = result.Duration,
                    Distance = result.Distance,
                    Has_Return = result.Has_Return,
                    Addresses = result.Addresses

                } };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                return new Response<OrderResult> { Message = ServiceMessage.Error };
            }
        }
    }
}
