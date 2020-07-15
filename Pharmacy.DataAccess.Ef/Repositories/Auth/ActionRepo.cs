using Pharmacy.Domain;
using Elk.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class ActionRepo : EfGenericRepo<Action>
    {
        public ActionRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}