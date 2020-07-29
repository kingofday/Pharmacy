using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Elk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pharmacy.DataAccess.Ef.Resource;
using System.Security.Cryptography.X509Certificates;

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

        public Response<GetDrugsModel> GetAsDTO(DrugSearchFilter filter)
        {
            var q = _Drug.Where(x => !x.IsDeleted
            && x.IsActive
            && x.DrugAssets.Any(x => x.AttachmentType == AttachmentType.DrugThumbnailImage)
            && x.DrugPrices.Any(x => x.IsDefault))
                .Include(x => x.DrugAssets)
                .Include(x => x.DrugPrices)
                .ThenInclude(x => x.Unit)
                .AsQueryable().AsNoTracking();
            var currentDT = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(filter.Name))
                q = q.Where(x => x.NameFa.Contains(filter.Name) || x.NameEn.Contains(filter.Name) || x.UniqueId.Contains(filter.Name));
            if (filter.CategoryId != null)
                q = q.Where(x => x.DrugCategoryId == filter.CategoryId);
            switch (filter.Type)
            {
                case DrugFilterType.MostVisited:
                    q = q.OrderByDescending(x => x.ViewCount);
                    break;
                case DrugFilterType.Favorites:
                    q = q.OrderByDescending(x => x.LikeCount);
                    break;
                case DrugFilterType.BestSellers:
                    q = q.OrderByDescending(x => x.OrderDetails.Sum(o => o.Count));
                    break;
            }
            var result = q.Select(p => new DrugDTO
            {
                DrugId = p.DrugId,
                NameFa = p.NameFa,
                NameEn = p.NameEn,
                UniqueId = p.UniqueId,
                ThumbnailImageUrl = p.DrugAssets.First(x => x.AttachmentType == AttachmentType.DrugThumbnailImage).Url,
                Price = p.DrugPrices.FirstOrDefault(x => x.IsDefault).Price,
                DiscountPrice = p.DrugPrices.FirstOrDefault(x => x.IsDefault).DiscountPrice
            });
            var maxPrice = result.OrderByDescending(x => x.Price).FirstOrDefault()?.Price;
            if (filter.MinPrice != null)
                result = result.Where(x => x.Price >= filter.MinPrice);
            if (filter.MaxPrice != null && filter.MaxPrice != 0)
                result = result.Where(x => x.Price <= filter.MaxPrice);
            switch (filter.Type)
            {
                case DrugFilterType.PriceAsc:
                    result = result.OrderBy(x => x.Price);
                    break;
                case DrugFilterType.PriceDesc:
                    result = result.OrderByDescending(x => x.Price);
                    break;
                default:
                    result = result.OrderByDescending(x => x.DrugId);
                    break;

            }

            return new Response<GetDrugsModel>
            {
                IsSuccessful = true,
                Result = new GetDrugsModel
                {
                    MaxPrice = maxPrice ?? 0,
                    Items = result.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList()
                }
            };
        }

    }
}
