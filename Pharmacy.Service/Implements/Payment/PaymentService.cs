using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public class PaymentService : IPaymentService
    {
        readonly AppUnitOfWork _appUow;
        readonly IPaymentRepo _paymentRepo;
        public PaymentService(AppUnitOfWork appUOW)
        {
            _appUow = appUOW;
            _paymentRepo = appUOW.PaymentRepo;
        }

        public async Task<IResponse<Payment>> Add(CreateTransactionRequest model, string transId, int gatewayId)
        {
            var paymentModel = new PaymentAddModel().CopyFrom(model);
            paymentModel.TransactionId = transId;
            paymentModel.GatewayId = gatewayId;
            return await Add(paymentModel);
        }
        public async Task<IResponse<Payment>> Add(PaymentAddModel model)
        {
            var payment = new Payment
            {
                OrderId = model.OrderId,
                PaymentGatewayId = model.GatewayId,
                PaymentStatus = PaymentStatus.Canceled,
                Price = model.Amount,
                TransactionId = model.TransactionId
            };
            await _paymentRepo.AddAsync(payment);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Payment>
            {
                IsSuccessful = saveResult.IsSuccessful,
                Message = saveResult.IsSuccessful ? string.Empty : saveResult.Message,
                Result = payment
            };
        }

        public async Task<IResponse<Payment>> Add(Payment model)
        {
            await _paymentRepo.AddAsync(model);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Payment>
            {
                IsSuccessful = saveResult.IsSuccessful,
                Message = saveResult.IsSuccessful ? string.Empty : saveResult.Message,
                Result = model
            };
        }

        public async Task<IResponse<Payment>> FindAsync(string transactionId)
        {
            var payment = await _paymentRepo.FirstOrDefaultAsync(new QueryFilter<Payment>
            {
                Conditions = x => x.TransactionId == transactionId
            });
            return new Response<Payment>
            {
                IsSuccessful = payment != null,
                Message = payment == null ? ServiceMessage.RecordNotExist : string.Empty,
                Result = payment
            };
        }

        public async Task<IResponse<Payment>> GetDetails(int paymentId)
        {
            var payment = await _paymentRepo.FirstOrDefaultAsync(new QueryFilter<Payment>
            {
                Conditions = x => x.PaymentId == paymentId,
                IncludeProperties = new System.Collections.Generic.List<System.Linq.Expressions.Expression<System.Func<Payment, object>>> {
                x=>x.Order,
                x=>x.Order.Address.User,
                x=>x.PaymentGateway
            }
            });
            return new Response<Payment>
            {
                IsSuccessful = payment != null,
                Message = payment == null ? ServiceMessage.RecordNotExist : string.Empty,
                Result = payment
            };
        }

        public async Task<IResponse<Payment>> Update(string transactionId, PaymentStatus status)
        {
            var payment = await _paymentRepo.FirstOrDefaultAsync(new QueryFilter<Payment>
            {
                Conditions = x => x.TransactionId == transactionId
            });
            if (payment == null) return new Response<Payment> { Message = ServiceMessage.RecordNotExist };
            payment.PaymentStatus = status;
            var updateResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Payment>
            {
                IsSuccessful = updateResult.IsSuccessful,
                Message = updateResult.Message,
                Result = payment
            };
        }

        public PaymentModel Get(PaymentSearchFilter filter) => _paymentRepo.GetItemsAndCount(filter);
    }
}
