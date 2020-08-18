using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IUnitService
    {
        Task<IResponse<Unit>> AddAsync(Unit model);
        Task<IResponse<bool>> DeleteAsync(int UnitId);
        Task<IResponse<Unit>> FindAsync(int id);
        PagingListDetails<Unit> Get(UnitSearchFilter filter);
        Task<IResponse<Unit>> UpdateAsync(Unit model);
    }
}