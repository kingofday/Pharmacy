using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;
using System.Collections.Generic;
using System.Linq.Expressions;
using Pharmacy.InfraStructure;
using Microsoft.AspNetCore.Http;

namespace Pharmacy.Service
{
    public class DrugStoreService : IDrugStoreService
    {
        readonly AppUnitOfWork _appUow;
        readonly AuthUnitOfWork _authUow;
        readonly IGenericRepo<DrugStore> _drugStoreRepo;
        readonly IAttachmentService _attchSrv;
        public DrugStoreService(AppUnitOfWork appUOW, AuthUnitOfWork authUow, IAttachmentService attchSrv)
        {
            _appUow = appUOW;
            _authUow = authUow;
            _drugStoreRepo = appUOW.DrugStoreRepo;
            _attchSrv = attchSrv;
        }

        public IResponse<DrugStoreModel> GetNearest(LocationDTO model)
        {
            var items = _drugStoreRepo.Get(selector: x => new DrugStoreModel
            {
                DrugStoreId = x.DrugStoreId,
                Name = x.Name,
                Lat = x.Address.Latitude,
                Lng = x.Address.Longitude
            },
                conditions: null,
                orderBy: o => o.OrderBy(x => x.DrugStoreId),
                includeProperties: new List<Expression<Func<DrugStore, object>>> { x => x.Address });
            if (items == null || !items.Any())
                return new Response<DrugStoreModel> { Message = ServiceMessage.RecordNotExist };
            Parallel.ForEach(items, (item) =>
            {
                item.Distance = GeoLocation.GetDistanceBetweenPoints(item.Lat, item.Lng, model.Lat, model.Lng);
            });
            return new Response<DrugStoreModel>
            {
                IsSuccessful = true,
                Result = items.OrderBy(x => x.Distance).First()
            };
        }
        //public async Task<IResponse<LocationDTO>> GetLocationAsync(int id)
        //{
        //    var addressId = await _drugStoreRepo.FirstOrDefaultAsync(x => x.AddressId, x => x.DrugStoreId == id && x.IsActive, includeProperties: null);//await _PharmacyRepo.FirstOrDefaultAsync(x => x.AddressId, x => x.PharmacyId == id && x.IsActive, includeProperties: null);
        //    if (addressId == null) return new Response<LocationDTO> { Message = ServiceMessage.RecordNotExist };
        //    var address = await _appUow.AddressRepo.FindAsync(addressId);
        //    if (address == null) return new Response<LocationDTO> { Message = ServiceMessage.RecordNotExist };
        //    return new Response<LocationDTO>
        //    {
        //        IsSuccessful = true,
        //        Result = new LocationDTO
        //        {
        //            Lat = address.Latitude,
        //            Lng = address.Longitude
        //        }
        //    };
        //}
        //public async Task<IResponse<DrugStoreDTO> FindAsDtoAsync(int id)
        //{
        //    try
        //    {
        //        var Pharmacy = await _PharmacyRepo.FindAsync(id);
        //        if (Pharmacy == null) return new Response<PharmacyDTO>
        //        {
        //            IsSuccessful = false,
        //            Message = ServiceMessage.RecordNotExist
        //        };

        //        return new Response<PharmacyDTO>
        //        {
        //            IsSuccessful = true,
        //            Result = new PharmacyDTO
        //            {
        //                Name = Pharmacy.FullName,
        //                LogoUrl = Pharmacy.ProfilePictureUrl
        //            }
        //        };
        //    }
        //    catch (Exception e)
        //    {

        //        return new Response<PharmacyDTO> { };
        //    }

        //}

        public async Task<IResponse<DrugStore>> FindAsync(int id)
        {
            var Pharmacy = await _drugStoreRepo.FirstOrDefaultAsync(conditions: x => x.DrugStoreId == id,
                           includeProperties: new List<Expression<Func<DrugStore, object>>> { x => x.Address });
            if (Pharmacy == null) return new Response<DrugStore> { Message = ServiceMessage.RecordNotExist };

            return new Response<DrugStore> { Result = Pharmacy, IsSuccessful = true };
        }

        public PagingListDetails<DrugStore> Get(DrugStoreSearchFilter filter)
        {
            Expression<Func<DrugStore, bool>> conditions = x => !x.IsDeleted;
            if (filter != null)
            {
                if (filter.UserId != null)
                    conditions = conditions.And(x => x.UserId == filter.UserId);
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    conditions = conditions.And(x => x.Name.Contains(filter.Name));
            }

            return _drugStoreRepo.Get(conditions, filter, x => x.OrderByDescending(i => i.DrugStoreId), new List<Expression<Func<DrugStore, object>>> {
                x=>x.User
            });
        }

        public async Task<IResponse<bool>> DeleteAsync(int id)
        {
            var Pharmacy = await _drugStoreRepo.FindAsync(id);
            if (Pharmacy == null) return new Response<bool> { Message = ServiceMessage.RecordNotExist };
            _drugStoreRepo.Delete(Pharmacy);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public async Task<IResponse<DrugStore>> SignUp(DrugStoreSignUpModel model)
        {
            using var tb = _appUow.Database.BeginTransaction();
            var mobileNumber = long.Parse(model.MobileNumber);
            var user = await _appUow.UserRepo.FirstOrDefaultAsync(conditions: x => x.MobileNumber == mobileNumber, null);

            var cdt = DateTime.Now;
            var drugStore = new DrugStore
            {
                Status = DrugStoreStatus.Registered,
                Name = model.Name,//crawl.FullName,
                IsActive = true,
                User = user ?? new User
                {
                    FullName = model.FullName,
                    IsActive = true,
                    MobileNumber = mobileNumber,
                    LastLoginDateMi = cdt,
                    LastLoginDateSh = PersianDateTime.Now.ToString(PersianDateTimeFormat.Date),
                    InsertDateMi = cdt,
                    Password = HashGenerator.Hash(mobileNumber.ToString()),
                    NewPassword = HashGenerator.Hash(mobileNumber.ToString()),
                    MustChangePassword = false,
                    UserStatus = UserStatus.Added
                }
            };
            await _drugStoreRepo.AddAsync(drugStore);
            var savePharmacy = await _appUow.ElkSaveChangesAsync();
            if (!savePharmacy.IsSuccessful)
            {
                tb.Rollback();
                return new Response<DrugStore> { Message = savePharmacy.Message };
            }
            if (user == null)
            {
                await _authUow.UserInRoleRepo.AddAsync(new UserInRole
                {
                    UserId = drugStore.UserId,
                    RoleId = model.RoleId ?? 0
                });
                var saveUserInRole = await _authUow.ElkSaveChangesAsync();
                if (!saveUserInRole.IsSuccessful) tb.Rollback();
                else tb.Commit();
                return new Response<DrugStore>
                {
                    IsSuccessful = saveUserInRole.IsSuccessful,
                    Result = drugStore,
                    Message = savePharmacy.Message
                };
            }
            tb.Commit();
            return new Response<DrugStore>
            {
                IsSuccessful = true,
                Result = drugStore
            };

        }

        public IEnumerable<DrugStore> GetAll(Guid userId)
        => _drugStoreRepo.Get(x => !x.IsDeleted && x.UserId == userId, o => o.OrderByDescending(x => x.DrugStoreId), null);

        public IDictionary<object, object> Search(string searchParameter, Guid? userId, int take = 10)
            => _drugStoreRepo.Get(conditions: x => !x.IsDeleted && x.Name.Contains(searchParameter) && userId == null ? true : x.UserId == userId,
                new PagingParameter
                {
                    PageNumber = 1,
                    PageSize = 6
                },
                o => o.OrderByDescending(x => x.DrugStoreId))
            .Items
            .ToDictionary(k => (object)k.DrugStoreId, v => (object)v.Name);

        public async Task<IResponse<DrugStore>> UpdateAsync(DrugStoreUpdateModel model)
        {
            var Pharmacy = await _appUow.DrugStoreRepo.FirstOrDefaultAsync(conditions: x => x.DrugStoreId == model.DrugStoreId,
                includeProperties: new List<Expression<Func<DrugStore, object>>> { x => x.Address });
            if (Pharmacy == null) return new Response<DrugStore> { Message = ServiceMessage.RecordNotExist };
            if (Pharmacy.Address == null)
            {
                await _appUow.DrugStoreAddressRepo.AddAsync(new DrugStoreAddress
                {
                    DrugStoreId = Pharmacy.DrugStoreId,
                    Latitude = model.Address.Latitude,
                    Longitude = model.Address.Longitude,
                    Details = model.Address.Details
                });
                var addAddress = await _appUow.ElkSaveChangesAsync();
                if (!addAddress.IsSuccessful) return new Response<DrugStore> { Message = addAddress.Message };
            }
            else
            {
                Pharmacy.Address.Latitude = model.Address.Latitude;
                Pharmacy.Address.Longitude = model.Address.Longitude;
                Pharmacy.Address.Details = model.Address.Details;
                _appUow.DrugStoreAddressRepo.Update(Pharmacy.Address);
            }
            if (model.Logo != null)
            {
                var save = await _attchSrv.Save(AttachmentType.DrugStoreLogo, new List<IFormFile> { model.Logo }, model.AppDir);
                if (!save.IsSuccessful)
                    return new Response<DrugStore> { Message = save.Message };
                Pharmacy.Address = new DrugStoreAddress().CopyFrom(save.Result);
            }
            _drugStoreRepo.Update(Pharmacy);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<DrugStore> { Result = Pharmacy, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<DrugStore>> UpdateAsync(DrugStoreAdminUpdateModel model)
        {
            var drugStore = await _appUow.DrugStoreRepo.FirstOrDefaultAsync(conditions: x => x.DrugStoreId == model.DrugStoreId,
                includeProperties: new List<Expression<Func<DrugStore, object>>> { x => x.Address });
            if (drugStore == null) return new Response<DrugStore> { Message = ServiceMessage.RecordNotExist };
            if (drugStore.Address == null)
            {
                await _appUow.DrugStoreAddressRepo.AddAsync(new DrugStoreAddress
                {
                    DrugStoreId = model.DrugStoreId,
                    Latitude = model.Address.Latitude,
                    Longitude = model.Address.Longitude,
                    Details = model.Address.Details
                });
                var addAddress = await _appUow.ElkSaveChangesAsync();
                if (!addAddress.IsSuccessful) return new Response<DrugStore> { Message = addAddress.Message };
            }
            else
            {
                drugStore.Address.Latitude = model.Address.Latitude;
                drugStore.Address.Longitude = model.Address.Longitude;
                drugStore.Address.Details = model.Address.Details;
                _appUow.DrugStoreAddressRepo.Update(drugStore.Address);
            }
            drugStore.Name = model.Name;
            drugStore.IsActive = model.IsActive;
            if (model.Logo != null)
            {
                //var dir = $"/Files/{model.DrugStoreId}";
                //if (!FileOperation.CreateDirectory(model.Root + dir))
                //    return new Response<Drug> { Message = ServiceMessage.SaveFileFailed };
                //var relativePath = $"{dir}/logo_{Guid.NewGuid().ToString().Replace("-", "_")}{Path.GetExtension(model.Logo.FileName)}";
                //using (var stream = File.Create($"{model.Root}{relativePath.Replace("/", "\\")}"))
                //    await model.Logo.CopyToAsync(stream);
                //Pharmacy.ProfilePictureUrl = $"{model.BaseDomain}{relativePath}";
            }
            _drugStoreRepo.Update(drugStore);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<DrugStore> { Result = drugStore, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<bool>> DeleteFile(string baseDomain, string root, int id)
        {
            var Pharmacy = await _drugStoreRepo.FindAsync(id);
            if (Pharmacy == null) return new Response<bool> { Message = ServiceMessage.RecordNotExist };
            //var url = Pharmacy.ProfilePictureUrl;
            //Pharmacy.ProfilePictureUrl = null;
            _drugStoreRepo.Update(Pharmacy);
            var update = await _appUow.ElkSaveChangesAsync();
            if (!update.IsSuccessful) return new Response<bool> { Message = update.Message };
            try
            {
                //if (url.StartsWith(baseDomain) && File.Exists(root + url.Replace(baseDomain, "")))
                //    File.Delete(root + url.Replace(baseDomain, ""));
                return new Response<bool> { IsSuccessful = true };
            }
            catch (Exception e)
            {
                FileLoger.Error(e);
                return new Response<bool> { Message = ServiceMessage.Error };
            }
        }

        public async Task<bool> CheckOwner(int PharmacyId, Guid userId) => await _drugStoreRepo.AnyAsync(x => x.DrugStoreId == PharmacyId && x.UserId == userId);

        public List<DrugStoreDTO> GetAsDTO()
            => _drugStoreRepo.Get(selector: x => new DrugStoreDTO
            {
                DrugStoreId = x.DrugStoreId,
                Name = x.Name,
                ImageUrl = x.DrugStoreAssets.First().Url
            },
                conditions: x => x.DrugStoreAssets.Any(),
                pagingParameter: new PagingParameter
                {
                    PageNumber = 1,
                    PageSize = 15
                },
                orderBy: o => o.OrderByDescending(x => x.DrugStoreId)).Items;
    }
}
