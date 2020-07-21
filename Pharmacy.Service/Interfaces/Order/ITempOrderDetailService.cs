using System;
using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public interface ITempBasketItemService
    {
        Task<IResponse<Guid>> AddRangeAsync(IList<TempBasketItem> model);
        Task<IResponse<bool>> DeleteAsync(Guid basketId);
        PagingListDetails<TempBasketItemModel> Get(TempBasketItemSearchFilter filter);
        List<TempBasketItem> GetDetails(Guid basketId);
        IResponse<IList<TempBasketItemDTO>> Get(Guid basketId);
    }
}