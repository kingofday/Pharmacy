using Elk.Core;

namespace Pharmacy.Domain
{
    public interface IDrugRepo : IGenericRepo<Drug>, IScopedInjection
    {
        Response<SingleDrugDTO> GetSingle(int id);
        Response<PagingListDetails<DrugDTO>> GetAsDTO(DrugSearchFilter filter);
    }
}
