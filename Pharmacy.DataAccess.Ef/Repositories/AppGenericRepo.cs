using Elk.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class AppGenericRepo<T> : EfGenericRepo<T> where T : class
    {
        public AppGenericRepo(AppDbContext appDbContext) : base(appDbContext) { }
    }
}