using Pharmacy.Domain;
using Elk.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class NotificationRepo : EfGenericRepo<Notification>
    {
        public NotificationRepo(AppDbContext appContext) : base(appContext)
        { }


    }
}
