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
            var items = _appContext.Set<TempOrderDetail>()
                .Include(x => x.DrugPrice)
                .ThenInclude(x => x.Drug)
                .ThenInclude(x => x.DrugAssets)
                .Where(x => x.BasketId == basketId)
                .AsNoTracking().Select(x => new TempOrderDetailDTO
                {
                    ItemId = x.TempOrderDetailId,
                    DrugId = x.DrugId,
                    Count = x.Count,
                    Price = x.Price,
                    PriceId = x.DrugPriceId,
                    DiscountPrice = 0,
                    NameFa = x.DrugPrice.Drug.NameFa,
                    NameEn = x.DrugPrice.Drug.NameEn,
                    TumbnailImageUrl = x.DrugPrice.Drug.DrugAssets.Any(x => x.AttachmentType == AttachmentType.DrugThumbnailImage) ? x.DrugPrice.Drug.DrugAssets.First(x => x.AttachmentType == AttachmentType.DrugThumbnailImage).Url : null
                }).ToList();
            return new Response<IList<TempOrderDetailDTO>>
            {
                IsSuccessful = true,
                Result = items
            };
        }
    }
}
