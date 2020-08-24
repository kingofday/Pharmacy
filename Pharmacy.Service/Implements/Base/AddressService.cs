using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;
using System.Collections.Generic;
using System.Linq.Expressions;

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
            var addresses = _addressRepo.Get(new PagedListFilterModel<UserAddress, AddressDTO>
            {
                Selector = x => new AddressDTO
                {
                    Id = x.UserAddressId,
                    Fullname = x.Fullname,
                    MobileNumber = x.MobileNumber.ToString(),
                    Lat = x.Latitude,
                    Lng = x.Longitude,
                    Details = x.Details
                },
                Conditions = x => x.UserId == userId,
                PagingParameter = new PagingParameter
                {
                    PageNumber = 1,
                    PageSize = 3
                },
                OrderBy = o => o.OrderByDescending(x => x.UserAddressId)
            });
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

        public async Task<Response<int>> AddAsync(Guid userId, AddressDTO model)
        {
            var addr = new UserAddress
            {
                UserId = userId,
                Latitude = model.Lat,
                Longitude = model.Lng,
                IsDefault = true,
                Fullname = model.Fullname,
                MobileNumber = string.IsNullOrWhiteSpace(model.MobileNumber)?0:long.Parse(model.MobileNumber),
                Details = model.Details
            };
            await _addressRepo.AddAsync(addr);
            var add = await _appUow.ElkSaveChangesAsync();
            return new Response<int>
            {
                Result = addr.UserAddressId,
                Message = add.IsSuccessful ? null : ServiceMessage.Error,
                IsSuccessful = add.IsSuccessful
            };
        }

        public async Task<Response<int>> UpdateAsync(Guid userId, AddressDTO model)
        {
            var addr = await _addressRepo.FindAsync(model.Id);
            if (addr.UserId != userId)
                return new Response<int> { Message = ServiceMessage.NotAllowedOperation };
            addr.Fullname = model.Fullname;
            addr.MobileNumber = long.Parse(model.MobileNumber);
            _addressRepo.Update(addr);
            var update = await _appUow.ElkSaveChangesAsync();
            return new Response<int>
            {
                Result = addr.UserAddressId,
                Message = update.IsSuccessful ? null : ServiceMessage.Error,
                IsSuccessful = update.IsSuccessful
            };
        }

        public async Task<Response<bool>> DeleteAsync(Guid userId, int id)
        {
            var addr = await _addressRepo.FirstOrDefaultAsync(new BaseFilterModel<UserAddress>
            {
                Conditions = x => x.UserId == userId && x.UserAddressId == id
            });
            if (addr == null) return new Response<bool> { Message = ServiceMessage.RecordNotExist };
            _addressRepo.Delete(addr);
            var delete = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Result = delete.IsSuccessful,
                Message = delete.IsSuccessful ? null : ServiceMessage.Error,
                IsSuccessful = delete.IsSuccessful
            };
        }
    }
}
