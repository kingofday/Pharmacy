using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Elk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class PaymentRepo : EfGenericRepo<Payment>, IPaymentRepo
    {
        readonly AppDbContext _appContext;
        public PaymentRepo(AppDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }

        public PaymentModel GetItemsAndCount(PaymentSearchFilter filter)
        {
            var q = _appContext.Set<Payment>()
                .AsNoTracking()
                .Include(x=>x.PaymentGateway)
                .Include(x=>x.Order)
                .Include(x=>x.Order.User)
                .AsQueryable();
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.FromDateSh))
                {
                    var dt = PersianDateTime.Parse(filter.FromDateSh).ToDateTime();
                    q = q.Where(x => x.InsertDateMi >= dt);
                }
                if (!string.IsNullOrWhiteSpace(filter.ToDateSh))
                {
                    var dt = PersianDateTime.Parse(filter.ToDateSh).ToDateTime();
                    q = q.Where(x => x.InsertDateMi <= dt);
                }
                if (!string.IsNullOrWhiteSpace(filter.TransactionId))
                    q = q.Where(x => x.TransactionId == filter.TransactionId);
                if (filter.PaymentStatus != null)
                    q = q.Where(x => x.PaymentStatus == filter.PaymentStatus);
            }
            return new PaymentModel
            {
                PagedList = q.ToPagingListDetails(filter),
                TotalPrice = q.Sum(x => x.Price)
            };

        }

    }
}
