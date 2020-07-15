using System;
using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public interface ITempOrderDetailService
    {
        Task<IResponse<Guid>> AddRangeAsync(IList<TempOrderDetail> model);
        Task<IResponse<bool>> DeleteAsync(Guid basketId);
        PagingListDetails<TempOrderDetailModel> Get(TempOrderDetailSearchFilter filter);
        List<TempOrderDetail> GetDetails(Guid basketId);
        IResponse<IList<TempOrderDetailDTO>> Get(Guid basketId);
    }
}