using Elk.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class AuthGenericRepo<T> : EfGenericRepo<T> where T : class
    {
        public AuthGenericRepo(AuthDbContext authDbContext) : base(authDbContext) { }
    }
}