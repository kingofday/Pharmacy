using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Elk.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.DataAccess.Ef
{
    public class TempBasketItemRepo : EfGenericRepo<TempBasketItem>, ITempBasketItemRepo
    {
        readonly private AppDbContext _appContext;
        public TempBasketItemRepo(AppDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
        }

        public PagingListDetails<TempBasketItemModel> GetBaskets(TempBasketItemSearchFilter filter)
        {
            var q = _appContext.Set<TempBasketItem>().AsQueryable();
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
                    if (isGuid) q = q.Where(x => x.TempBasketId == id);
                }
            }
            var groups = q.GroupBy(x => new
            {
                x.TempBasketId,
                x.InsertDateSh
            })
            .Select(x => new TempBasketItemModel
            {
                BasketId = x.Key.TempBasketId,
                InsertDateSh = x.Key.InsertDateSh,
                TotalPrice = x.Sum(i => i.TotalPrice)
            });
            var items = groups.OrderByDescending(x => x.InsertDateSh)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();
            var count = q.Count();
            return new PagingListDetails<TempBasketItemModel>
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                Items = new PagingList<TempBasketItemModel>(items, count, filter),
                TotalCount = count
            };

        }

        public IResponse<IList<TempBasketItemDTO>> GetItems(Guid basketId)
        {
            var items = _appContext.Set<TempBasketItem>()
                .Include(x => x.Drug)
                .ThenInclude(x => x.DrugAssets)
                .Where(x => x.TempBasketId == basketId)
                .AsNoTracking().Select(x => new TempBasketItemDTO
                {
                    ItemId = x.TempBasketItemId,
                    DrugId = x.DrugId,
                    Count = x.Count,
                    Price = x.Price,
                    DiscountPrice = 0,
                    NameFa = x.Drug.NameFa,
                    NameEn = x.Drug.NameEn,
                    ThumbnailImageUrl = x.Drug.DrugAssets.Any(x => x.AttachmentType == AttachmentType.DrugThumbnailImage) ? x.Drug.DrugAssets.First(x => x.AttachmentType == AttachmentType.DrugThumbnailImage).Url : null
                }).ToList();
            return new Response<IList<TempBasketItemDTO>>
            {
                IsSuccessful = true,
                Result = items
            };
        }
    }
}
