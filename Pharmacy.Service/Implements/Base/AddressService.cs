using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;

namespace Pharmacy.Service
{
    public class AddressService : IAddressService
    {
        readonly AppUnitOfWork _appUow;
        readonly IGenericRepo<Address> _addressRepo;
        public AddressService(AppUnitOfWork appUOW, IGenericRepo<Address> addressRepo)
        {
            _appUow = appUOW;
            _addressRepo = addressRepo;
        }

        public IResponse<PagingListDetails<AddressDTO>> Get(Guid userId)
        {
            var currentDT = DateTime.Now;
            var addresses = _addressRepo.Get(selector: a => new AddressDTO
            {
                Id = a.AddressId,
                Lat = a.Latitude,
                Lng = a.Longitude,
                Address = a.AddressDetails
            },
            conditions: x => x.UserId == userId,
            pagingParameter: new PagingParameter
            {
                PageNumber = 1,
                PageSize = 3
            },
            orderBy: o => o.OrderByDescending(x => x.AddressId));
            return new Response<PagingListDetails<AddressDTO>>
            {
                Result = addresses,
                IsSuccessful = true
            };
        }

        public async Task<IResponse<Address>> FindAsync(int id)
        {
            var addr = await _addressRepo.FindAsync(id);
            if (addr == null) return new Response<Address> { Message = ServiceMessage.RecordNotExist };
            else return new Response<Address> { IsSuccessful = true, Result = addr };
        }
    }
}
