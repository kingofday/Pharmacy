using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IRoleService : IScopedInjection
    {
        Task<IResponse<Role>> AddAsync(Role model);
        Task<IResponse<Role>> UpdateAsync(Role model);
        Task<IResponse<bool>> DeleteAsync(int roleId);
        Task<IResponse<Role>> FindAsync(int roleId);
        PagingListDetails<Role> Get(RoleSearchFilter filter);
    }
}