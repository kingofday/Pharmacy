using System;
using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IAddressService
    {
        IResponse<PagingListDetails<AddressDTO>> Get(Guid userId);
        Task<IResponse<Address>> FindAsync(int id);
    }
}