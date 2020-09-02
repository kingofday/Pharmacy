using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;

namespace Pharmacy.Service
{
    public class GatewayFactory : IGatewayFactory
    {
        readonly IGenericRepo<PaymentGateway> _paymentGatewayRepo;
        readonly IPaymentService _paymentSrv;
        readonly AppUnitOfWork _appUow;
        public GatewayFactory(IGenericRepo<PaymentGateway> paymentGatewayRepo, AppUnitOfWork appUow, IPaymentService paymentSrv)
        {
            _paymentGatewayRepo = paymentGatewayRepo;
            _paymentSrv = paymentSrv;
            _appUow = appUow;
        }

        public async Task<IResponse<(IGatewayService Service, PaymentGateway Gateway)>> GetInsance(int gatewayId)
        {
            var paymentGateway = await _paymentGatewayRepo.FirstOrDefaultAsync(new BaseFilterModel<PaymentGateway> { Conditions = x => x.PaymentGatewayId == gatewayId });
            if (paymentGateway.Name == null) return new Response<(IGatewayService Service, PaymentGateway Gateway)> { Message = ServiceMessage.RecordNotExist };
            switch (paymentGateway.Name)
            {
                default:
                    return new Response<(IGatewayService Service, PaymentGateway Gateway)> { IsSuccessful = true, Result = (new HillaPayService(_appUow, _paymentSrv), paymentGateway) };
            }

        }
    }
}
