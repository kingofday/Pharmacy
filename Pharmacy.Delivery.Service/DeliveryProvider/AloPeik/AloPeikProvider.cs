using System;
using Elk.Core;
using Elk.Http;
using System.Text;
using System.Linq;
using Pharmacy.Domain;
using System.Net.Http;
using Pharmacy.InfraStructure;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Pharmacy.Delivery.Service.Resource;

namespace Pharmacy.Delivery.Service
{
    public static class AloPeikProvider
    {
        public static async Task<AloPeikUser> Authenticate()
        {
            var authResult = await HttpRequestTools.GetAsync<AloPeikResult<AloPeikUser>>(GlobalVariables.DeliveryProviders.AloPeik.Url, Encoding.UTF8);
            if (authResult.Status == "success") return authResult.Object;

            return null;
        }

        public static async Task<IResponse<dynamic>> AddressInquiry(LocationDTO location)
        {
            var result = new Response<dynamic>();
            try
            {
                var responseBody = string.Empty;
                using (var webClient = new HttpClient())
                {
                    webClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariables.DeliveryProviders.AloPeik.Token);
                    responseBody = await webClient.GetStringAsync($"{GlobalVariables.DeliveryProviders.AloPeik.Url}/locations?latlng={location.Lat},{location.Lng}");

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
                        result.Message = ServiceMessage.Success;
                    }
                    else
                        result.Message = ServiceMessage.Error;
                }

                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                result.Message = ServiceMessage.Exception;
                return result;
            }
        }

        public static async Task<PriceInquiryResult> PriceInquiry(LocationDTO origin, LocationDTO destination, bool cashed, bool hasReturn)
        {
            var result = new PriceInquiryResult();
            try
            {
                #region Create Request Bode
                var model = new
                {
                    transport_type = AloPeikTransportType.motor_taxi.ToString(),
                    addresses = new List<dynamic> {
                        new { type = AloPeikAddressType.origin.ToString(), lat =  origin.Lat, lng = origin.Lng},
                        new { type = AloPeikAddressType.destination.ToString(), lat =  destination.Lat, lng = destination.Lng}
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
                            DeliveryProviderId = 1,
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
                    else
                        result = null;
                }

                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e);

                return null;
            }
        }

        public static async Task<AloPeikOrderResult> RegisterOrder(DeliveryOrderLocationDTO origin, DeliveryOrderLocationDTO destination, bool cashed, bool hasReturn, string extraParams)
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
