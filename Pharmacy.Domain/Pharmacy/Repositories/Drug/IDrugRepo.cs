using Elk.Core;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public interface IDrugRepo : IGenericRepo<Drug>, IScopedInjection
    {
        Response<SingleDrugDTO> GetSingle(int id);
        Response<GetDrugsModel> GetAsDTO(DrugSearchFilter filter);
        //Response<List<GetDrugPriceList>> GetPrices(List<int> ids);
        //Response<List<DrugPriceDTO>> GetSingleDrugPrice(int id);
    }
}
