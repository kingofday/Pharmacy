using System;
using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public interface IDrugService
    {
        IResponse<PagingListDetails<DrugDTO>> GetAsDto(DrugSearchFilter filter);
        Task<IResponse<Drug>> FindAsync(int id);
        Response<SingleDrugDTO> GetSingle(int id);

        Task<(bool Changed, IEnumerable<OrderItemDTO> Items)> CheckChanges(IEnumerable<OrderItemDTO> items);

        Task<IResponse<Drug>> AddAsync(DrugAddModel model);

        Task<IResponse<Drug>> FindWithAssetsAsync(int id);

        Task<IResponse<Drug>> UpdateAsync(DrugAddModel model);

        Task<IResponse<bool>> DeleteAsync(string baseDomain, string root, int id);

        PagingListDetails<Drug> Get(DrugSearchFilter filter);

        Response<List<DrugDTO>> Get(string q);

        IList<DrugSearchResult> Search(string searchParameter, int take = 10);
    }
}