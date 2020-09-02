using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Pharmacy.Service.Resource;

namespace Pharmacy.Service
{
    public class TagService : ITagService
    {
        readonly AppUnitOfWork _appUow;

        public TagService(AppUnitOfWork appUOW)
        {
            _appUow = appUOW;
        }


        public async Task<IResponse<Tag>> AddAsync(Tag model)
        {
            await _appUow.TagRepo.AddAsync(model);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Tag>
            {
                Result = model,
                IsSuccessful = saveResult.IsSuccessful,
                Message = saveResult.Message
            };
        }

        public async Task<IResponse<bool>> UpdateAsync(Tag model)
        {
            var item = await _appUow.TagRepo.FindAsync(model.TagId);
            if (item.IsNull()) return new Response<bool>
            {
                IsSuccessful = false,
                Message = ServiceMessage.RecordNotExist
            };
            item.Name = model.Name;

            _appUow.TagRepo.Update(item);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                IsSuccessful = saveResult.IsSuccessful,
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful
            };
        }

        public async Task<IResponse<int>> DeleteAsync(int id)
        {
            _appUow.TagRepo.Delete(new Tag { TagId = id });
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<int>
            {
                IsSuccessful = saveResult.IsSuccessful,
                Message = saveResult.Message,
                Result = id
            };
        }

        public async Task<IResponse<Tag>> FindAsync(int id, bool includeAttchs = false)
        {
            var item = await _appUow.TagRepo.FindAsync(id);

            return new Response<Tag>
            {
                IsSuccessful = item != null,
                Message = item == null ? ServiceMessage.RecordNotExist : string.Empty,
                Result = item
            };
        }

        public PagingListDetails<Tag> Get(TagSearchFilter filter)
        {
            Expression<Func<Tag, bool>> conditions = x => true;
            if (!string.IsNullOrWhiteSpace(filter.TitleF)) conditions = conditions.And(x => x.Name.Contains(filter.TitleF));
            return _appUow.TagRepo.Get(new BasePagedListFilterModel<Tag> { Conditions = conditions, PagingParameter = filter, OrderBy = x => x.OrderByDescending(x => x.Name) });
        }

        public PagingListDetails<Tag> GetForDashbord(TagSearchFilter filter)
        {
            Expression<Func<Tag, bool>> conditions = x => true;
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.TitleF))
                    conditions = conditions.And(x => x.Name.Contains(filter.TitleF));
            }

            return _appUow.TagRepo.Get(new BasePagedListFilterModel<Tag> { Conditions = conditions, PagingParameter = filter, OrderBy = x => x.OrderByDescending(i => i.TagId) });
        }

    }
}
