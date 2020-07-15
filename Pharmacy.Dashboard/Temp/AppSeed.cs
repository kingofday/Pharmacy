using Pharmacy.DataAccess.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Dashboard.Temp
{
    public class AppSeed
    {
        private readonly AuthDbContext _authDb;
        private readonly AppDbContext _appDb;

        public AppSeed(AuthDbContext db, AppDbContext appDb)
        {
            _authDb = db;
            _appDb = appDb;
        }


    }
}
