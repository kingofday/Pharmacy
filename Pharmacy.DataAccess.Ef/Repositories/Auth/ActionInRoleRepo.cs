using Pharmacy.Domain;
using Elk.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class ActionInRoleRepo : EfGenericRepo<ActionInRole>
    {
        public ActionInRoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}