using Pharmacy.Domain;
using Elk.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class TagRepo : EfGenericRepo<Tag>, ITagRepo
    {
        public TagRepo(AppDbContext appContext) : base(appContext)
        { }
    }
}