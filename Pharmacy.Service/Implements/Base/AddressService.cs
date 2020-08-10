using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public class AddressService : IAddressService
    {
        readonly AppUnitOfWork _appUow;
        readonly IGenericRepo<UserAddress> _addressRepo;
        public AddressService(AppUnitOfWork appUOW, IGenericRepo<UserAddress> addressRepo)
        {
            _appUow = appUOW;
            _addressRepo = addressRepo;
        }

        public Response<List<AddressDTO>> Get(Guid userId)
        {
            var currentDT = DateTime.Now;
            var addresses = _addressRepo.Get(selector: a => new AddressDTO
            {
                Id = a.UserAddressId,
                Lat = a.Latitude,
                Lng = a.Longitude,
                Details = a.Details
            },
            conditions: x => x.UserId == userId,
            pagingParameter: new PagingParameter
            {
                PageNumber = 1,
                PageSize = 3
            },
            orderBy: o => o.OrderByDescending(x => x.UserAddressId));
            return new Response<List<AddressDTO>>
            {
                Result = addresses.Items,
                IsSuccessful = true
            };
        }

        public async Task<IResponse<UserAddress>> FindAsync(int id)
        {
            var addr = await _addressRepo.FindAsync(id);
            if (addr == null) return new Response<UserAddress> { Message = ServiceMessage.RecordNotExist };
            else return new Response<UserAddress> { IsSuccessful = true, Result = addr };
        }
    }
}
