using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IGatewayFactory
    {
        Task<IResponse<(IGatewayService Service, PaymentGateway Gateway)>> GetInsance(int gatewayId);
    }
}