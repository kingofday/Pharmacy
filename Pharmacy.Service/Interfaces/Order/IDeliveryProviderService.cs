using Elk.Core;
using Pharmacy.Domain;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public interface IDeliveryProviderService
    {
        Response<List<DeliveryDTO>> GetAllAsDTO();
    }
}