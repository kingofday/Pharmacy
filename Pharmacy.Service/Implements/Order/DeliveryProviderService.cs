using Elk.Cache;
using Elk.Core;
using Pharmacy.DataAccess.Ef;
using Pharmacy.Domain;
using Pharmacy.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Service
{
    public class DeliveryProviderService : IDeliveryProviderService
    {
        readonly IMemoryCacheProvider _cache;
        readonly AppUnitOfWork _appUOW;
        public DeliveryProviderService(AppUnitOfWork appUOW, IMemoryCacheProvider cache)
        {
            _appUOW = appUOW;
            _cache = cache;
        }
        public Response<List<DeliveryDTO>> GetAllAsDTO()
        {
            var result = (List<DeliveryDTO>)_cache.Get(GlobalVariables.CacheSettings.DeluveryProviders);
            if (result == null)
            {
                result = EnumExtension.GetEnumElements<DeliveryType>()
                    .Select(x => new DeliveryDTO
                    {
                        Id = (int)Enum.Parse(typeof(DeliveryType), x.Name),
                        Name = x.Description
                    }).ToList();
                _cache.Add(GlobalVariables.CacheSettings.DeluveryProviders, result, DateTime.Now.AddHours(2));
            }


            return new Response<List<DeliveryDTO>>
            {
                IsSuccessful = true,
                Result = result
            };
        }

    }
}
