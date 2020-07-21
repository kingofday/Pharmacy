using Elk.Core;
using System;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public interface ITempBasketItemRepo : IGenericRepo<TempBasketItem>, IScopedInjection
    {
        PagingListDetails<TempBasketItemModel> GetBaskets(TempBasketItemSearchFilter filter);
        IResponse<IList<TempBasketItemDTO>> GetItems(Guid basketId);
    }
}
