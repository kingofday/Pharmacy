using System;
using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public interface IAddressService
    {
        Response<List<AddressDTO>> Get(Guid userId);
        Task<IResponse<UserAddress>> FindAsync(int id);
        Task<Response<int>> AddAsync(Guid userId, AddressDTO model);
        Task<Response<bool>> DeleteAsync(Guid userId, int id);
    }
}