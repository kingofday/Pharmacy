using Elk.Core;

namespace Pharmacy.Domain
{
    public interface IDrugRepo : IGenericRepo<Drug>, IScopedInjection
    {
        IResponse<SingleDrugDTO> GetSingle(int id);
        IResponse<PagingListDetails<DrugDTO>> GetAsDTO(DrugSearchFilter filter);
    }
}
