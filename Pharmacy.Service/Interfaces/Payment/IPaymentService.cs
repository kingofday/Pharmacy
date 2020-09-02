using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IPaymentService
    {
        Task<IResponse<Payment>> Add(CreateTransactionRequest model, string transId, int gatewayId);
        Task<IResponse<Payment>> Add(PaymentAddModel model);
        Task<IResponse<Payment>> Add(Payment model);
        Task<IResponse<Payment>> FindAsync(string transactionId);
        Task<IResponse<Payment>> GetDetails(int paymentId);
        Task<IResponse<Payment>> Update(string transactionId, PaymentStatus status);
        PaymentModel Get(PaymentSearchFilter filter);
    }
}