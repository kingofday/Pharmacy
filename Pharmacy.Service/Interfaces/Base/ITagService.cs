using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface ITagService
    {
        PagingListDetails<Tag> Get(TagSearchFilter filter);
        PagingListDetails<Tag> GetForDashbord(TagSearchFilter filter);
        Task<IResponse<Tag>> AddAsync(Tag model);
        Task<IResponse<Tag>> FindAsync(int id, bool includeAttchs = false);
        Task<IResponse<bool>> UpdateAsync(Tag model);
        Task<IResponse<int>> DeleteAsync(int id);
    }
}