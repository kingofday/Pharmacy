using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;

namespace Pharmacy.Service
{
    public class DrugCategoryService : IDrugCategoryService
    {
        readonly AppUnitOfWork _appUow;
        readonly IGenericRepo<DrugCategory> _drugCategoryRepo;
        public DrugCategoryService(AppUnitOfWork appUOW, IGenericRepo<DrugCategory> drugCategoryRepo)
        {
            _appUow = appUOW;
            _drugCategoryRepo = drugCategoryRepo;
        }

        public PagingListDetails<DrugCategory> Get(DrugCategorySearchFilter filter)
        {
            Expression<Func<DrugCategory, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    conditions = x => x.Name.Contains(filter.Name);
            }

            return _drugCategoryRepo.Get(new BasePagedListFilterModel<DrugCategory> { Conditions = conditions, PagingParameter = filter, OrderBy = x => x.OrderByDescending(u => u.DrugCategoryId) });
        }

        public IList<DrugCategory> GetAll(DrugCategorySearchFilter filter)
        {
            Expression<Func<DrugCategory, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    conditions = x => x.Name.Contains(filter.Name);
            }
            return _drugCategoryRepo.Get(new BaseListFilterModel<DrugCategory>
            {
                Conditions = conditions,
                OrderBy = x => x.OrderBy(u => u.OrderPriority)
            });
        }

        public IDictionary<object, object> Search(string searchParameter, int take = 10)
                => _drugCategoryRepo.Get(new BaseListFilterModel<DrugCategory>
                {
                    Conditions = x => x.Name.Contains(searchParameter)
                })
                .OrderByDescending(x => x.Name)
                .Take(take)
                .ToDictionary(k => (object)k.DrugCategoryId, v => (object)v.Name);

        public async Task<IResponse<DrugCategory>> FindAsync(int id)
        {
            var item = await _drugCategoryRepo.FindAsync(id);
            if (item == null) return new Response<DrugCategory> { Message = ServiceMessage.RecordNotExist };

            return new Response<DrugCategory> { Result = item, IsSuccessful = true };
        }

        public async Task<IResponse<DrugCategory>> AddAsync(DrugCategory model)
        {
            await _drugCategoryRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<DrugCategory> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<DrugCategory>> UpdateAsync(DrugCategory model)
        {
            var findedRole = await _drugCategoryRepo.FindAsync(model.DrugCategoryId);
            if (findedRole == null) return new Response<DrugCategory> { Message = ServiceMessage.RecordNotExist };

            findedRole.Name = model.Name;
            findedRole.Icon = model.Icon;
            findedRole.OrderPriority = model.OrderPriority;

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<DrugCategory> { Result = findedRole, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(int id)
        {
            _drugCategoryRepo.Delete(new DrugCategory { DrugCategoryId = id });
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public Response<List<DrugCategoryDTO>> Get()
            => new Response<List<DrugCategoryDTO>>
            {
                IsSuccessful = true,
                Result = _drugCategoryRepo.Get(new ListFilterModel<DrugCategory, DrugCategoryDTO>
                {
                    Selector = x => new DrugCategoryDTO
                    {
                        CategoryId = x.DrugCategoryId,
                        Name = x.Name

                    },
                    Conditions = x => x.ParentId == null,
                    OrderBy = o => o.OrderBy(x => x.OrderPriority)
                })
            };
    }
}
