using Pharmacy.Domain;
using System.Threading.Tasks;
using Elk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Elk.Core;

namespace Pharmacy.DataAccess.Ef
{
    public class UserRepo : EfGenericRepo<User>, IUserRepo
    {
        readonly AppDbContext _appContext;
        public UserRepo(AppDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }
    }
}
