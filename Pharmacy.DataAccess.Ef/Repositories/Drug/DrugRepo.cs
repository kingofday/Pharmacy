using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Elk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pharmacy.DataAccess.Ef.Resource;

namespace Pharmacy.DataAccess.Ef
{
    public class DrugRepo : EfGenericRepo<Drug>, IDrugRepo
    {
        readonly DbSet<Drug> _Drug;
        readonly AppDbContext _appContext;
        public DrugRepo(AppDbContext appContext) : base(appContext)
        {
            _appContext = appContext;
            _Drug = _appContext.Set<Drug>();
        }
        public Response<SingleDrugDTO> GetSingle(int id)
        {
            var drug = _Drug.Where(x => !x.IsDeleted
            && x.IsActive
            && x.DrugPrices.Any(x => x.IsDefault)
            && x.DrugId == id)
                .Include(x => x.DrugAssets)
                .Include(x => x.DrugTags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.DrugPrices)
                .ThenInclude(x => x.Unit)
                .AsNoTracking()
                .FirstOrDefault();
            if (drug == null) new Response<SingleDrugDTO> { Message = Strings.ItemNotFound };
            return new Response<SingleDrugDTO>
            {
                IsSuccessful = true,
                Result = new SingleDrugDTO
                {
                    DrugId = drug.DrugId,
                    NameEn = drug.NameEn,
                    NameFa = drug.NameFa,
                    Prices = drug.DrugPrices?.Select(p => new DrugPriceDTO
                    {
                        Name = p.Unit.Name,
                        Price = p.Price,
                        DiscountPrice = p.DiscountPrice
                    }).ToList(),
                    Slides = drug.DrugAssets?.Select(x => x.Url).ToList(),
                    Tags = drug.DrugTags?.Select(t => new DrugTagDTO
                    {
                        TagId = t.TagId,
                        Name = t.Tag.Name
                    }).ToList()
                }
            };
        }

        public IResponse<PagingListDetails<DrugDTO>> GetAsDTO(DrugSearchFilter filter)
        {
            var q = _Drug.Where(x => !x.IsDeleted && x.IsActive && x.DrugPrices.Any(x => x.IsDefault))
                .Include(x => x.DrugAssets)
                .Include(x => x.DrugPrices)
                .ThenInclude(x => x.Unit)
                .AsQueryable().AsNoTracking();
            var currentDT = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(filter.Name)) 
                q = q.Where(x => x.NameFa.Contains(filter.Name) || x.NameEn.Contains(filter.Name) || x.UniqueId.Contains(filter.Name));

            switch (filter.Category)
            {
                case DrugFilterCategory.MostVisited:
                    q = q.OrderByDescending(x => x.ViewCount);
                    break;
                case DrugFilterCategory.Favorites:
                    q = q.OrderByDescending(x => x.LikeCount);
                    break;
                case DrugFilterCategory.BestSellers:
                    q = q.OrderByDescending(x => x.OrderDetails.Count());
                    break;
                default:
                    q = q.OrderByDescending(x => x.DrugId);
                    break;
            }
            var result = q.Select(p => new DrugDTO
            {
                DrugId = p.DrugId,
                NameFa = p.NameFa,
                NameEn = p.NameEn,
                UniqueId = p.UniqueId,
                TumbnailImageUrl = p.DrugAssets.Any(x => x.AttachmentType == AttachmentType.DrugThumbnailImage) ? p.DrugAssets.First(x => x.AttachmentType == AttachmentType.DrugThumbnailImage).Url : null,
                Price = p.DrugPrices.FirstOrDefault(x => x.IsDefault).Price,
                DiscountPrice = p.DrugPrices.FirstOrDefault(x => x.IsDefault).DiscountPrice
            }).ToPagingListDetails(filter);
            return new Response<PagingListDetails<DrugDTO>>
            {
                IsSuccessful = true,
                Result = result
            };
        }

    }
}
