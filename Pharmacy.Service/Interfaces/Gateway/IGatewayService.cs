using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IGatewayService
    {
        Task<IResponse<CreateTransactionReponse>> CreateTrasaction(CreateTransactionRequest model, object[] args);
        Task<IResponse<string>> VerifyTransaction(VerifyRequest model, object[] args);
    }
}