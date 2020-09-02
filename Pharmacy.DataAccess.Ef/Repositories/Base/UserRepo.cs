using Pharmacy.Domain;
using System.Threading.Tasks;
using Elk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Elk.Core;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace Pharmacy.DataAccess.Ef
{
    public class UserRepo : EfGenericRepo<User>, IUserRepo
    {
        readonly AppDbContext _appContext;
        public UserRepo(AppDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }

        public async Task<List<MenuSPModel>> GetUserMenu(Guid userId) =>
         await _appContext.MenuSPModel.FromSqlRaw("EXEC [Auth].[GetUserMenu] {0}", userId).ToListAsync();
    }
}
