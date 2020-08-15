using Pharmacy.Domain;

namespace Pharmacy.Service
{
    public class DeliveryAgentFactory : IDeliveryAgentFactory
    {
        public DeliveryAgentService Get(DeliveryType type)
        {
            switch (type)
            {
                case DeliveryType.Peyk:
                    return new AloPeykService();
                default:
                    return new PostService();
            }
        }
    }
}
