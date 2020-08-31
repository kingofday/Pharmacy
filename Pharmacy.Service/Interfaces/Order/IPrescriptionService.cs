using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IPrescriptionService
    {
        Task<Response<int>> Add(AddPrescriptionModel model);

        PagingListDetails<Prescription> Get(PrescriptionSearchFilter filter);

        Task<IResponse<Prescription>> FindAsync(int id);
        Task<IResponse<Prescription>> UpdateAsync(Prescription model);
    }
}