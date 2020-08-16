using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IPrescriptionService
    {
        Task<Response<int>> Add(AddPrescriptionModel model);
    }
}