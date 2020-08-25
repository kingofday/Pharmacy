using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Pharmacy.Service.Resource;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Pharmacy.Service
{
    public class DrugService : IDrugService
    {
        readonly AppUnitOfWork _appUow;
        readonly IAttachmentService _attchService;
        readonly IConfiguration _configuration;
        readonly IDrugRepo _drugRepo;
        public DrugService(AppUnitOfWork appUOW,
            IAttachmentService DrugAttachmentService,
            IConfiguration configuration)
        {
            _appUow = appUOW;
            _drugRepo = appUOW.DrugRepo;
            _attchService = DrugAttachmentService;
            _configuration = configuration;
        }

        public Response<GetDrugsModel> GetAsDto(DrugSearchFilter filter) => _drugRepo.GetAsDTO(filter);

        public Response<SingleDrugDTO> GetSingle(int id) => _drugRepo.GetSingle(id);

        public async Task<IResponse<Drug>> FindAsync(int id)
        {
            var drug = await _drugRepo.FirstOrDefaultAsync(new BaseFilterModel<Drug>
            {
                Conditions = x => x.DrugId == id,
                IncludeProperties = new List<Expression<Func<Drug, object>>> { x => x.Properties, x => x.DrugAttachments }
            });
            if (drug == null) return new Response<Drug> { Message = ServiceMessage.RecordNotExist };
            drug.DrugTags = _appUow.DrugTagRepo.Get(new BaseListFilterModel<DrugTag>
            {
                Conditions = x => x.DrugId == id,
                OrderBy = o => o.OrderByDescending(x => x.DrugTagId),
                IncludeProperties = new List<Expression<Func<DrugTag, object>>> { x => x.Tag }
            });
            return new Response<Drug>
            {
                IsSuccessful = true,
                Result = drug
            };

        }

        public (bool Changed, IEnumerable<OrderItemDTO> Items) CheckChanges(IEnumerable<OrderItemDTO> items)
        {
            var drugs = _drugRepo.Get(new BaseListFilterModel<Drug>
            {
                Conditions = x => items.Select(x => x.DrugId).Contains(x.DrugId),
                OrderBy = o => o.OrderByDescending(x => x.DrugId)
            });
            bool changed = false;
            foreach (var item in items)
            {
                var drug = drugs.FirstOrDefault(x => x.DrugId == item.DrugId);
                if (drug == null || !drug.IsActive)
                {
                    changed = true;
                    item.Count = 0;
                    continue;
                }
                else if(drug.Price != item.Price || drug.DiscountPrice != item.Discount)
                {
                    changed = true;
                    item.Price = drug.Price;
                    item.Discount = drug.DiscountPrice;
                }

            }
            return (changed, items);
        }

        public async Task<IResponse<Drug>> AddAsync(DrugAddModel model)
        {
            var drug = new Drug().CopyFrom(model);
            if (model.TagIds != null && model.TagIds.Any())
                drug.DrugTags = new List<DrugTag>(model.TagIds.Select(x => new DrugTag { TagId = x }));
            if (model.Properties != null)
                drug.Properties = model.Properties;
            if (model.Files != null && model.Files.Count != 0)
            {
                var save = await _attchService.Save(AttachmentType.DrugThumbnailImage, model.Files, model.AppDir);
                if (!save.IsSuccessful) return new Response<Drug> { Message = save.Message };
                drug.DrugAttachments = save.Result.Select(x => new DrugAttachment().CopyFrom(x)).ToList();
                await _drugRepo.AddAsync(drug);
                var add = await _appUow.ElkSaveChangesAsync();
                return new Response<Drug> { Result = drug, IsSuccessful = add.IsSuccessful, Message = add.Message };
            }
            else
            {
                await _drugRepo.AddAsync(drug);
                var add = await _appUow.ElkSaveChangesAsync();
                return new Response<Drug> { Result = drug, IsSuccessful = add.IsSuccessful, Message = add.Message };
            }
        }

        public async Task<IResponse<Drug>> FindWithAssetsAsync(int id)
        {
            var Drug = await _drugRepo.FindAsync(id);
            if (Drug == null) return new Response<Drug> { Message = ServiceMessage.RecordNotExist };

            return new Response<Drug> { Result = Drug, IsSuccessful = true };
        }

        public async Task<IResponse<Drug>> UpdateAsync(DrugAddModel model)
        {
            var drug = await _drugRepo.FindAsync(model.DrugId);
            if (drug == null) return new Response<Drug> { Message = ServiceMessage.RecordNotExist };
            drug.Price = model.Price;
            drug.DiscountPrice = model.DiscountPrice;
            drug.NameFa = model.NameFa;
            drug.NameEn = model.NameEn;
            drug.IsActive = model.IsActive;
            drug.ShortDescription = model.ShortDescription;
            drug.Description = model.Description;
            drug.DrugCategoryId = model.DrugCategoryId;
            if (model.Properties != null)
                drug.Properties = model.Properties;
            #region Tags
            if (model.TagIds == null) model.TagIds = new List<int>();
            var tags = _appUow.DrugTagRepo.Get(new BaseListFilterModel<DrugTag> { Conditions = x => x.DrugId == model.DrugId, OrderBy = o => o.OrderByDescending(x => x.DrugTagId) });
            _appUow.DrugTagRepo.DeleteRange(tags.Where(x => !model.TagIds.Contains(x.TagId)).ToList());
            if (model.TagIds != null && model.TagIds.Any())
                drug.DrugTags = new List<DrugTag>(model.TagIds.Where(x => !tags.Select(t => t.TagId).Contains(x)).Select(x => new DrugTag { TagId = x }));
            #endregion

            if (model.Files != null && model.Files.Count != 0)
            {
                var save = await _attchService.Save(AttachmentType.DrugThumbnailImage, model.Files, model.AppDir);
                if (!save.IsSuccessful) return new Response<Drug> { Message = save.Message };
                drug.DrugAttachments = save.Result.Select(x => new DrugAttachment().CopyFrom(x)).ToList();
                _drugRepo.Update(drug);
                var update = await _appUow.ElkSaveChangesAsync();
                if (!update.IsSuccessful) _attchService.DeleteRange(model.AppDir, save.Result);
                return new Response<Drug> { Result = drug, IsSuccessful = update.IsSuccessful, Message = update.Message };
            }
            else
            {
                _drugRepo.Update(drug);
                var update = await _appUow.ElkSaveChangesAsync();
                return new Response<Drug> { Result = drug, IsSuccessful = update.IsSuccessful, Message = update.Message };
            }
        }

        public async Task<IResponse<bool>> DeleteAsync(string appDir, int id)
        {
            var Drug = await _drugRepo.FindAsync(id);
            var urls = _appUow.DrugAttachmentRepo.Get(new ListFilterModel<DrugAttachment, string>
            {
                Selector = x => x.Url,
                Conditions = x => x.DrugAttachmentId == id,
                OrderBy = x => x.OrderBy(o => o.DrugAttachmentId)
            });
            _drugRepo.Delete(Drug);
            var delete = await _appUow.ElkSaveChangesAsync();
            if (delete.IsSuccessful) _attchService.DeleteRange(appDir, urls);
            return new Response<bool>
            {
                Message = delete.Message,
                Result = delete.IsSuccessful,
                IsSuccessful = delete.IsSuccessful,
            };
        }

        public PagingListDetails<Drug> Get(DrugSearchFilter filter)
        {
            Expression<Func<Drug, bool>> conditions = x => !x.IsDeleted;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    conditions = conditions.And(x => x.NameFa.Contains(filter.Name) || x.NameEn.Contains(filter.Name));
                if (filter.CategoryId != null)
                    conditions = conditions.And(x => x.DrugCategoryId == filter.CategoryId);
                if (filter.UniqueId != null)
                    conditions = conditions.And(x => x.UniqueId.Contains(filter.UniqueId));
            }

            return _drugRepo.Get(new BasePagedListFilterModel<Drug>
            {
                Conditions = conditions,
                PagingParameter = filter,
                OrderBy = x => x.OrderByDescending(i => i.DrugId)
            });
        }

        public Response<List<DrugDTO>> Get(string q)
               => new Response<List<DrugDTO>>
               {
                   Result = _drugRepo.GetAsDTO(new DrugSearchFilter
                   {
                       Name = q,
                       PageNumber = 1,
                       PageSize = 3
                   }).Result.Items,
                   IsSuccessful = true
               };

        public IList<DrugSearchResult> Search(string searchParameter, int take = 10)
                => _drugRepo.Get(new PagedListFilterModel<Drug, DrugSearchResult>
                {
                    Selector = x => new DrugSearchResult
                    {
                        Id = x.DrugId,
                        NameFa = x.NameFa,
                        NameEn = x.NameEn,
                    },
                    Conditions = x => !x.IsDeleted && (x.NameFa.Contains(searchParameter) || x.NameEn.Contains(searchParameter)),
                    PagingParameter = new PagingParameter
                    {
                        PageNumber = 1,
                        PageSize = 6
                    },
                    OrderBy = o => o.OrderByDescending(x => x.NameFa)
                }).Items;

        //----------------------------------------------new

        public async Task<IResponse<string>> DeleteAttachment(string appDir, int assetId)
        {
            var asset = await _appUow.DrugAttachmentRepo.FindAsync(assetId);
            if (asset == null) return new Response<string> { Message = ServiceMessage.RecordNotExist };
            var url = asset.Url;
            _appUow.DrugAttachmentRepo.Delete(asset);
            var delete = await _appUow.ElkSaveChangesAsync();
            if (delete.IsSuccessful)
                _attchService.Delete(appDir, url);
            return new Response<string>
            {
                IsSuccessful = delete.IsSuccessful,
                Message = delete.IsSuccessful ? null : ServiceMessage.Error
            };

        }

        public async Task<IResponse<string>> DeleteProp(int propId)
        {
            _appUow.DrugPropertyRepo.Delete(new DrugProperty { DrugPropertyId = propId });
            var delete = await _appUow.ElkSaveChangesAsync();
            return new Response<string>
            {
                IsSuccessful = delete.IsSuccessful,
                Message = delete.IsSuccessful ? null : ServiceMessage.Error
            };

        }
    }
}
