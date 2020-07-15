using Pharmacy.Domain;
using Elk.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class RoleRepo : EfGenericRepo<Role>
    {
        public RoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}