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
        readonly IDrugAssetService _DrugAssetService;
        readonly IConfiguration _configuration;
        readonly IDrugRepo _drugRepo;
        public DrugService(AppUnitOfWork appUOW,
            IDrugAssetService DrugAssetService,
            IConfiguration configuration)
        {
            _appUow = appUOW;
            _drugRepo = appUOW.DrugRepo;
            _DrugAssetService = DrugAssetService;
            _configuration = configuration;
        }

        public Response<GetDrugsModel> GetAsDto(DrugSearchFilter filter) => _drugRepo.GetAsDTO(filter);

        public Response<SingleDrugDTO> GetSingle(int id) => _drugRepo.GetSingle(id);

        public async Task<IResponse<Drug>> FindAsync(int id)
        {
            var drug = await _drugRepo.FirstOrDefaultAsync(conditions: x => x.DrugId == id);
            if (drug == null) return new Response<Drug> { Message = ServiceMessage.RecordNotExist };
            drug.DrugTags = _appUow.DrugTagRepo.Get(conditions: x => x.DrugId == id,
                o => o.OrderByDescending(x => x.DrugTagId)
            , includeProperties: new List<Expression<Func<DrugTag, object>>> {
                x=>x.Tag
            });
            return new Response<Drug>
            {
                IsSuccessful = true,
                Result = drug
            };

        }

        public async Task<(bool Changed, IEnumerable<OrderItemDTO> Items)> CheckChanges(IEnumerable<OrderItemDTO> items)
        {
            var drugs = _drugRepo.Get(conditions: x => items.Select(x => x.Id).Contains(x.DrugId),
            orderBy: o => o.OrderByDescending(x => x.DrugId));
            bool changed = false;
            foreach (var item in items)
            {
                var drug = drugs.FirstOrDefault(x => x.DrugId == item.Id);
                if (drug == null || !drug.IsActive)
                {
                    changed = true;
                    item.Count = 0;
                    continue;
                }

            }
            return (changed, items);
        }

        public async Task<IResponse<Drug>> AddAsync(DrugAddModel model)
        {
            var Drug = new Drug().CopyFrom(model);
            if (model.TagIds != null && model.TagIds.Any())
                Drug.DrugTags = new List<DrugTag>(model.TagIds.Select(x => new DrugTag { TagId = x }));
            if (model.Files != null && model.Files.Count != 0)
            {
                var getAssets = await _DrugAssetService.SaveRange(model);
                if (!getAssets.IsSuccessful) return new Response<Drug> { Message = getAssets.Message };
                Drug.DrugAssets = getAssets.Result;
                await _drugRepo.AddAsync(Drug);
                var add = await _appUow.ElkSaveChangesAsync();
                if (!add.IsSuccessful) _DrugAssetService.DeleteRange(getAssets.Result);
                return new Response<Drug> { Result = Drug, IsSuccessful = add.IsSuccessful, Message = add.Message };
            }
            else
            {
                await _drugRepo.AddAsync(Drug);
                var add = await _appUow.ElkSaveChangesAsync();
                return new Response<Drug> { Result = Drug, IsSuccessful = add.IsSuccessful, Message = add.Message };
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
            var Drug = await _drugRepo.FindAsync(model.DrugId);
            if (Drug == null) return new Response<Drug> { Message = ServiceMessage.RecordNotExist };
            Drug.NameFa = model.NameFa;
            Drug.NameEn = model.NameEn;
            Drug.IsActive = model.IsActive;
            Drug.Description = model.Description;
            Drug.DrugCategoryId = model.DrugCategoryId;
            #region Tags
            if (model.TagIds == null) model.TagIds = new List<int>();
            var tags = _appUow.DrugTagRepo.Get(conditions: x => x.DrugId == model.DrugId, orderBy: o => o.OrderByDescending(x => x.DrugTagId));
            _appUow.DrugTagRepo.DeleteRange(tags.Where(x => !model.TagIds.Contains(x.TagId)).ToList());
            var ttt = model.TagIds.Where(x => !tags.Select(t => t.TagId).Contains(x)).ToList();
            if (model.TagIds != null && model.TagIds.Any())
                Drug.DrugTags = new List<DrugTag>(model.TagIds.Where(x => !tags.Select(t => t.TagId).Contains(x)).Select(x => new DrugTag { TagId = x }));
            #endregion
            
            _drugRepo.Update(Drug);
            if (model.Files != null && model.Files.Count != 0)
            {
                var getAssets = await _DrugAssetService.SaveRange(model);
                if (!getAssets.IsSuccessful) return new Response<Drug> { Message = getAssets.Message };
                foreach (var asset in getAssets.Result) asset.DrugId = model.DrugId;
                await _appUow.DrugAssetRepo.AddRangeAsync(getAssets.Result);
                var update = await _appUow.ElkSaveChangesAsync();
                if (!update.IsSuccessful) _DrugAssetService.DeleteRange(getAssets.Result);
                return new Response<Drug> { Result = Drug, IsSuccessful = update.IsSuccessful, Message = update.Message };
            }
            else
            {
                var update = await _appUow.ElkSaveChangesAsync();
                return new Response<Drug> { Result = Drug, IsSuccessful = update.IsSuccessful, Message = update.Message };
            }
        }

        public async Task<IResponse<bool>> DeleteAsync(string baseDomain, string root, int id)
        {
            var Drug = await _drugRepo.FindAsync(id);
            var urls = _appUow.DrugAssetRepo.Get(x => new { x.Url, x.PhysicalPath }, x => x.DrugId == id, o => o.OrderBy(x => x.DrugId)).Select(x => (x.Url, x.PhysicalPath));
            _drugRepo.Delete(Drug);
            var delete = await _appUow.ElkSaveChangesAsync();
            if (delete.IsSuccessful) _DrugAssetService.DeleteFiles(baseDomain, urls);
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
            }

            return _drugRepo.Get(conditions, filter, x => x.OrderByDescending(i => i.DrugId));
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
                => _drugRepo.Get(x => new DrugSearchResult
                {
                    Id = x.DrugId,
                    NameFa = x.NameFa,
                    NameEn = x.NameEn,
                },
                    conditions: x => !x.IsDeleted && (x.NameFa.Contains(searchParameter) || x.NameEn.Contains(searchParameter)),
                    new PagingParameter
                    {
                        PageNumber = 1,
                        PageSize = 6
                    },
                    o => o.OrderByDescending(x => x.NameFa)).Items;

        //----------------------------------------------new

    }
}
