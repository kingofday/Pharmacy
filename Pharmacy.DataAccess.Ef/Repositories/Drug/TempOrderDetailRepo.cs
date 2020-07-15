using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Elk.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class TempOrderDetailRepo : EfGenericRepo<TempOrderDetail>, ITempOrderDetailRepo
    {
        readonly private AppDbContext _appContext;
        public TempOrderDetailRepo(AppDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }

        public PagingListDetails<TempOrderDetailModel> GetBaskets(TempOrderDetailSearchFilter filter)
        {
            var q = _appContext.Set<TempOrderDetail>().AsQueryable();
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.FromDateSh))
                {
                    var dt = PersianDateTime.Parse(filter.FromDateSh).ToDateTime();
                    q = q.Where(x => x.InsertDateMi >= dt);
                }
                if (!string.IsNullOrWhiteSpace(filter.ToDateSh))
                {
                    var dt = PersianDateTime.Parse(filter.FromDateSh).ToDateTime();
                    q = q.Where(x => x.InsertDateMi <= dt);
                }
                if (filter.BasketId != null)
                {
                    var isGuid = Guid.TryParse(filter.BasketId, out Guid id);
                    if (isGuid) q = q.Where(x => x.BasketId == id);
                }
            }
            var groups = q.GroupBy(x => new
            {
                x.BasketId,
                x.InsertDateSh
            })
            .Select(x => new TempOrderDetailModel
            {
                BasketId = x.Key.BasketId,
                InsertDateSh = x.Key.InsertDateSh,
                TotalPrice = x.Sum(i => i.TotalPrice)
            });
            var items = groups.OrderByDescending(x => x.InsertDateSh)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
            var count = q.Count();
            return new PagingListDetails<TempOrderDetailModel>
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Items = new PagingList<TempOrderDetailModel>(items, count, filter),
                TotalCount = count
            };

        }

        public IResponse<IList<TempOrderDetailDTO>> GetItems(Guid basketId)
        {
            var items = _appContext.Set<TempOrderDetail>().Include(x => x.Drug).Where(x => x.BasketId == basketId)
                .AsNoTracking().Select(x => new TempOrderDetailDTO
                {
                    ItemId = x.TempOrderDetailId,
                    Id = x.DrugId,
                    Count = x.Count,
                    Price = x.Price,
                    Discount = 0,
                    NameFa = x.Drug.NameFa,
                    NameEn = x.Drug.NameEn,
                    ImageUrl = x.Drug.DrugAssets.Any() ? x.Drug.DrugAssets[0].Url :null
                }).ToList();
            return new Response<IList<TempOrderDetailDTO>>{
                IsSuccessful = true,
                Result = items
            };
        }
    }
}
