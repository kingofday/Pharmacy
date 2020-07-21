using System;
using Elk.Core;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Service
{
    public class TempBasketItemService : ITempBasketItemService
    {
        readonly private ITempBasketItemRepo _TempBasketItemRepo;
        readonly private AppUnitOfWork _appUOW;
        public TempBasketItemService(AppUnitOfWork appUOW)
        {
            _appUOW = appUOW;
            _TempBasketItemRepo = appUOW.TempBasketItemRepo;
        }

        public async Task<IResponse<Guid>> AddRangeAsync(IList<TempBasketItem> model)
        {
            var id = Guid.NewGuid();
            for (var i = 0; i < model.Count; i++) model[i].TempBasketId = id;
            await _TempBasketItemRepo.AddRangeAsync(model);

            var save = await _appUOW.ElkSaveChangesAsync();
            return new Response<Guid> { Result = id, IsSuccessful = save.IsSuccessful, Message = save.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(Guid basketId)
        {
            var items = _TempBasketItemRepo.Get(x => x.TempBasketId == basketId, o => o.OrderByDescending(x => x.TempBasketId));
            if (items == null || !items.Any()) return new Response<bool> { IsSuccessful = true };
            _TempBasketItemRepo.DeleteRange(items);
            var saveResult = await _appUOW.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public PagingListDetails<TempBasketItemModel> Get(TempBasketItemSearchFilter filter) => _TempBasketItemRepo.GetBaskets(filter);

        public List<TempBasketItem> GetDetails(Guid basketId)
                        => _TempBasketItemRepo.Get(x => x.TempBasketId == basketId, o => o.OrderBy(x => x.TempBasketItemId), new List<Expression<Func<TempBasketItem, object>>> {
                            i=>i.DrugPrice.Drug
                        });

        public IResponse<IList<TempBasketItemDTO>> Get(Guid basketId) => _TempBasketItemRepo.GetItems(basketId);
    }
}