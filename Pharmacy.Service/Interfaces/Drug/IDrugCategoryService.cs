using Elk.Core;
using Pharmacy.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IDrugCategoryService
    {
        PagingListDetails<DrugCategory> Get(DrugCategorySearchFilter filter);
        IList<DrugCategory> GetAll(DrugCategorySearchFilter filter);
        IDictionary<object, object> Search(string searchParameter, int take = 10);
        Task<IResponse<DrugCategory>> AddAsync(DrugCategory model);
        Task<IResponse<DrugCategory>> UpdateAsync(DrugCategory model);
        Task<IResponse<bool>> DeleteAsync(int id);
        Task<IResponse<DrugCategory>> FindAsync(int id);
        Response<List<DrugCategoryDTO>> Get();
    }
}