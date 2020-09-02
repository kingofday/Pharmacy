using System;
using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public interface IDrugService
    {
        Response<GetDrugsModel> GetAsDto(DrugSearchFilter filter,string baseUrl);
        Task<IResponse<Drug>> FindAsync(int id);
        Response<SingleDrugDTO> GetSingle(int id,string baseUrl);

        (bool Changed, IEnumerable<OrderItemDTO> Items) CheckChanges(IEnumerable<OrderItemDTO> items);

        Task<IResponse<Drug>> AddAsync(DrugAddModel model);

        Task<IResponse<Drug>> FindWithAssetsAsync(int id);

        Task<IResponse<Drug>> UpdateAsync(DrugAddModel model);

        Task<IResponse<bool>> DeleteAsync(string appDir, int id);

        PagingListDetails<Drug> Get(DrugSearchFilter filter);

        Response<List<DrugDTO>> Get(string q, string baseUrl);

        IList<DrugSearchResult> Search(string searchParameter, int take = 10);

        Task<IResponse<string>> DeleteAttachment(string appDir, int assetId);

        Task<IResponse<string>> DeleteProp(int propId);
    }
}