using Pharmacy.Domain;
using Elk.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class DrugStoreRepo : EfGenericRepo<DrugStore>, IStoreRepo
    {
        public DrugStoreRepo(AppDbContext appContext) : base(appContext)
        { }

    }
}
