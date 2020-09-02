using Elk.Core;

namespace Pharmacy.Domain
{
    public interface IDrugRepo : IGenericRepo<Drug>, IScopedInjection
    {
        Response<SingleDrugDTO> GetSingle(int id, string baseUrl);
        Response<GetDrugsModel> GetAsDTO(DrugSearchFilter filter, string baseUrl);
        //Response<List<GetDrugPriceList>> GetPrices(List<int> ids);
        //Response<List<DrugPriceDTO>> GetSingleDrugPrice(int id);
    }
}
