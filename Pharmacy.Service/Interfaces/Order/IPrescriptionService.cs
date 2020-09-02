using Elk.Core;
using Pharmacy.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IPrescriptionService
    {
        Task<Response<int>> Add(AddPrescriptionModel model);
        PagingListDetails<Prescription> Get(PrescriptionSearchFilter filter);
        Task<IResponse<Prescription>> FindDetailsAsync(int id);
        Task<IResponse<Prescription>> UpdateAsync(Prescription model);
        Task<IResponse<List<PrescriptionItem>>> DeleteItem(int itemId);
        Task<IResponse<string>> SendLink(int id, string url);
        Response<List<DrugDTO>> GetItems(int id);
    }
}