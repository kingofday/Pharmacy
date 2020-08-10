using Elk.Core;
using Pharmacy.DataAccess.Ef;
using Pharmacy.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Service
{
    public class DeliveryProviderService : IDeliveryProviderService
    {
        readonly IGenericRepo<DeliveryProvider> _delProvRepo;
        readonly AppUnitOfWork _appUOW;
        public DeliveryProviderService(AppUnitOfWork appUOW)
        {
            _appUOW = appUOW;
            _delProvRepo = appUOW.DeliveryProviderRepo;
        }
        public Response<List<DeliveryDTO>> GetAllAsDTO()
            => new Response<List<DeliveryDTO>>
            {
                IsSuccessful = true,
                Result = _delProvRepo.Get(selector: x => new DeliveryDTO
                {
                    Id = x.DeliveryProviderId,
                    Name = x.Username
                },
                orderBy: o => o.OrderByDescending(x => x.DeliveryProviderId))
            };
    }
}
