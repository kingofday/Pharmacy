using Elk.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharmacy.Domain
{
    public interface IUserRepo : IGenericRepo<User>, IScopedInjection
    {
        Task<List<MenuSPModel>> GetUserMenu(Guid userId);
    }
}
