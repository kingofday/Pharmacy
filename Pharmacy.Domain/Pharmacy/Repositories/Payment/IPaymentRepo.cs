using Elk.Core;

namespace Pharmacy.Domain
{
    public interface IPaymentRepo : IGenericRepo<Payment>, IScopedInjection
    {
        PaymentModel GetItemsAndCount(PaymentSearchFilter filter);
    }
}
