using Elk.Core;
using System;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public interface ITempOrderDetailRepo : IGenericRepo<TempOrderDetail>, IScopedInjection
    {
        PagingListDetails<TempOrderDetailModel> GetBaskets(TempOrderDetailSearchFilter filter);
        IResponse<IList<TempOrderDetailDTO>> GetItems(Guid basketId);
    }
}
