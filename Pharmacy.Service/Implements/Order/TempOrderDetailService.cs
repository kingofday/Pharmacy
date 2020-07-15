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
    public class TempOrderDetailService : ITempOrderDetailService
    {
        readonly private ITempOrderDetailRepo _tempOrderDetailRepo;
        readonly private AppUnitOfWork _appUOW;
        public TempOrderDetailService(AppUnitOfWork appUOW)
        {
            _appUOW = appUOW;
            _tempOrderDetailRepo = appUOW.TempOrderDetailRepo;
        }

        public async Task<IResponse<Guid>> AddRangeAsync(IList<TempOrderDetail> model)
        {
            var id = Guid.NewGuid();
            for (var i = 0; i < model.Count; i++) model[i].BasketId = id;
            await _tempOrderDetailRepo.AddRangeAsync(model);

            var save = await _appUOW.ElkSaveChangesAsync();
            return new Response<Guid> { Result = id, IsSuccessful = save.IsSuccessful, Message = save.Message };
        }

        public async Task<IResponse<bool>> DeleteAsync(Guid basketId)
        {
            var items = _tempOrderDetailRepo.Get(x => x.BasketId == basketId, o => o.OrderByDescending(x => x.BasketId));
            if (items == null || !items.Any()) return new Response<bool> { IsSuccessful = true };
            _tempOrderDetailRepo.DeleteRange(items);
            var saveResult = await _appUOW.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public PagingListDetails<TempOrderDetailModel> Get(TempOrderDetailSearchFilter filter) => _tempOrderDetailRepo.GetBaskets(filter);

        public List<TempOrderDetail> GetDetails(Guid basketId)
                        => _tempOrderDetailRepo.Get(x => x.BasketId == basketId, o => o.OrderBy(x => x.TempOrderDetailId), new List<Expression<Func<TempOrderDetail, object>>> {
                            i=>i.Drug
                        });

        public IResponse<IList<TempOrderDetailDTO>> Get(Guid basketId) => _tempOrderDetailRepo.GetItems(basketId);
    }
}