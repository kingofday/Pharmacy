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

        public IResponse<DrugStoreModel> GetNearest(LocationDTO model, List<int> excludedStores = null)
        {
            var items = _drugStoreRepo.Get(new ListFilterModel<DrugStore, DrugStoreModel>
            {
                Selector = x => new DrugStoreModel
                {
                    UserId = x.UserId,
                    DrugStoreId = x.DrugStoreId,
                    Name = x.Name,
                    Latitude = x.Address.Latitude,
                    Longitude = x.Address.Longitude
                },
                Conditions = x => excludedStores == null ? true : !excludedStores.Contains(x.DrugStoreId),
                OrderBy = o => o.OrderBy(x => x.DrugStoreId),
                IncludeProperties = new List<Expression<Func<DrugStore, object>>> { x => x.Address }
            });
            if (items == null || !items.Any())
                return new Response<DrugStoreModel> { Message = ServiceMessage.RecordNotExist };
            foreach (var item in items)
            {
                item.Distance = GeoLocation.GetDistanceBetweenPoints(item.Latitude, item.Longitude, model.Latitude, model.Longitude);
            }
            return new Response<DrugStoreModel>
            {
                IsSuccessful = true,
                Result = items.OrderBy(x => x.Distance).First()
            };
        }

        public async Task<IResponse<DrugStore>> FindAsync(int id)
        {
            var Pharmacy = await _drugStoreRepo.FirstOrDefaultAsync(new BaseFilterModel<DrugStore>
            {
                Conditions = x => x.DrugStoreId == id,
                IncludeProperties = new List<Expression<Func<DrugStore, object>>> { x => x.Address, x => x.Attachments, x => x.User }
            });
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

            return _drugStoreRepo.Get(new BasePagedListFilterModel<DrugStore>
            {
                Conditions = conditions,
                PagingParameter = filter,
                OrderBy = x => x.OrderByDescending(i => i.DrugStoreId),
                IncludeProperties = new List<Expression<Func<DrugStore, object>>> { x => x.User }
            });
        }

        public List<DrugStoreDTO> GetAsDTO(string baseUrl)
            => _drugStoreRepo.Get(new PagedListFilterModel<DrugStore, DrugStoreDTO>
            {
                Selector = x => new DrugStoreDTO
                {
                    Name = x.Name,
                    DrugStoreId = x.DrugStoreId,
                    ImageUrl = x.Attachments.Select(x => baseUrl + x.Url).FirstOrDefault()
                },
                PagingParameter = new PagingParameter { PageNumber = 1, PageSize = 15 },
                OrderBy = o => o.OrderByDescending(x => x.Score),
                IncludeProperties = new List<Expression<Func<DrugStore, object>>> { x => x.Attachments }
            }).Items;

        public async Task<IResponse<bool>> DeleteAsync(int id, string appDir)
        {
            var item = await _drugStoreRepo.FirstOrDefaultAsync(new BaseFilterModel<DrugStore>
            {
                Conditions = x => x.DrugStoreId == id,
                IncludeProperties = new List<Expression<Func<DrugStore, object>>> { x => x.Address, x => x.Attachments }
            });
            if (item == null) return new Response<bool> { Message = ServiceMessage.RecordNotExist };
            _drugStoreRepo.Delete(item);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            if (saveResult.IsSuccessful && item.Attachments != null) _attchSrv.DeleteRange(appDir, item.Attachments.Select(x => x.Url).ToList());
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public IEnumerable<DrugStore> GetAll(Guid userId)
        => _drugStoreRepo.Get(new BaseListFilterModel<DrugStore>
        {
            Conditions = x => !x.IsDeleted && x.UserId == userId,
            OrderBy = o => o.OrderByDescending(x => x.DrugStoreId)
        });

        public IDictionary<object, object> Search(string searchParameter, Guid? userId, int take = 10)
            => _drugStoreRepo.Get(new BasePagedListFilterModel<DrugStore>
            {
                Conditions = x => !x.IsDeleted && x.Name.Contains(searchParameter) && userId == null ? true : x.UserId == userId,
                PagingParameter = new PagingParameter
                {
                    PageNumber = 1,
                    PageSize = 6
                },
                OrderBy = o => o.OrderByDescending(x => x.DrugStoreId)
            }).Items
            .ToDictionary(k => (object)k.DrugStoreId, v => (object)v.Name);

        public async Task<IResponse<DrugStore>> AddAsync(DrugStoreAdminModel model)
        {
            var save = await _attchSrv.Save(AttachmentType.DrugStoreLogo, model.Logo, model.AppDir);
            if (!save.IsSuccessful) return new Response<DrugStore> { Message = save.Message };
            var logo = new DrugStoreAttachment().CopyFrom(save.Result);
            var drugStore = new DrugStore
            {
                Name = model.Name,
                IsActive = model.IsActive,
                Status = model.Status,
                UserId = model.UserId,
                Address = model.Address,
                Attachments = new List<DrugStoreAttachment>
                {
                    logo
                }

            };
            await _drugStoreRepo.AddAsync(drugStore);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<DrugStore> { Result = drugStore, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<DrugStore>> UpdateAsync(DrugStoreUpdateModel model)
        {
            var Pharmacy = await _appUow.DrugStoreRepo.FirstOrDefaultAsync(new BaseFilterModel<DrugStore>
            {
                Conditions = x => x.DrugStoreId == model.DrugStoreId,
                IncludeProperties = new List<Expression<Func<DrugStore, object>>> { x => x.Address }
            });
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

        public async Task<IResponse<DrugStore>> UpdateAsync(DrugStoreAdminModel model)
        {
            var drugStore = await _appUow.DrugStoreRepo.FirstOrDefaultAsync(new BaseFilterModel<DrugStore>
            {
                Conditions = x => x.DrugStoreId == model.DrugStoreId,
                IncludeProperties = new List<Expression<Func<DrugStore, object>>> { x => x.Address }
            });
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
                var save = await _attchSrv.Save(AttachmentType.DrugStoreLogo, model.Logo, model.AppDir);
                if (!save.IsSuccessful) return new Response<DrugStore> { Message = save.Message };
                var logo = new DrugStoreAttachment().CopyFrom(save.Result);
                drugStore.Attachments = new List<DrugStoreAttachment>
                {
                    logo
                };
            }
            _drugStoreRepo.Update(drugStore);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<DrugStore> { Result = drugStore, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<string>> DeleteFile(string appDir, int assetId)
        {
            var asset = await _appUow.DrugStoreAttachmentRepo.FindAsync(assetId);
            if (asset == null) return new Response<string> { Message = ServiceMessage.RecordNotExist };
            var url = asset.Url;
            _appUow.DrugStoreAttachmentRepo.Delete(asset);
            var delete = await _appUow.ElkSaveChangesAsync();
            if (delete.IsSuccessful)
                _attchSrv.Delete(appDir, url);
            return new Response<string>
            {
                IsSuccessful = delete.IsSuccessful,
                Message = delete.IsSuccessful ? null : ServiceMessage.Error
            };
        }

        public async Task<bool> CheckOwner(int PharmacyId, Guid userId) => await _drugStoreRepo.AnyAsync(new BaseFilterModel<DrugStore>
        {
            Conditions = x => x.DrugStoreId == PharmacyId && x.UserId == userId
        });

        //public List<DrugStoreDTO> GetAsDTO()
        //    => _drugStoreRepo.Get(new PagedListFilterModel<DrugStore, DrugStoreDTO>
        //    {
        //        Selector = x => new DrugStoreDTO
        //        {
        //            DrugStoreId = x.DrugStoreId,
        //            Name = x.Name,
        //            //ImageUrl = x.DrugStoreAssets.First().Url
        //        },
        //        //Conditions = x => x.DrugStoreAssets.Any(),
        //        PagingParameter = new PagingParameter
        //        {
        //            PageNumber = 1,
        //            PageSize = 15
        //        },
        //        OrderBy = o => o.OrderByDescending(x => x.DrugStoreId)
        //    }).Items;
    }
}
