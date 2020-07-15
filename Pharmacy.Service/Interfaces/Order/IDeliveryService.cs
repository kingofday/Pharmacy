using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IDeliveryService
    {
        //Task<IResponse<GetDeliveryTypesDTO>> GetDeliveryTypes(int PharmacyId, LocationDTO location);
        //Task<IResponse<int>> GetDeliveryCost(int deliveryId, int PharmacyId, LocationDTO location);
        Task<IResponse<int>> Add(int orderId);
    }
}