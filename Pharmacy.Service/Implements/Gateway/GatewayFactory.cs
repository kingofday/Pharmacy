using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.Service.Resource;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public class GatewayFactory : IGatewayFactory
    {
        readonly IGenericRepo<PaymentGateway> _paymentGatewayRepo;
        public GatewayFactory(IGenericRepo<PaymentGateway> paymentGatewayRepo)
        {
            _paymentGatewayRepo = paymentGatewayRepo;
        }

        public async Task<IResponse<(IGatewayService Service, PaymentGateway Gateway)>> GetInsance(int gatewayId)
        {
            var paymentGateway = await _paymentGatewayRepo.FirstOrDefaultAsync(conditions: x => x.PaymentGatewayId == gatewayId, includeProperties: null);
            if (paymentGateway.Name == null) return new Response<(IGatewayService Service, PaymentGateway Gateway)> { Message = ServiceMessage.RecordNotExist };
            switch (paymentGateway.Name)
            {
                default:
                    return new Response<(IGatewayService Service, PaymentGateway Gateway)> { IsSuccessful = true, Result = (new HillaPayService(), paymentGateway) };
            }

        }
    }
}
