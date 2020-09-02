using Pharmacy.Domain;

namespace Pharmacy.Service
{
    public interface IDeliveryAgentFactory
    {
        DeliveryAgentService Get(DeliveryType type);
    }
}