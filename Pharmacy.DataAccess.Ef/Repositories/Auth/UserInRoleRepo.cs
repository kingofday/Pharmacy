using Pharmacy.Domain;
using Elk.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class UserInRoleRepo : EfGenericRepo<UserInRole>
    {
        public UserInRoleRepo(AuthDbContext authContext) : base(authContext)
        { }
    }
}
