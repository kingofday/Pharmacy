using System;
using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Elk.EntityFrameworkCore;
using System.Collections.Generic;
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
            && x.DrugId == id)
                .Include(x => x.DrugAttachments)
                .Include(x => x.Comments)
                .ThenInclude(x => x.User)
                .Include(x => x.Properties)
                .Include(x => x.DrugTags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.Unit)
                .Include(x => x.DrugCategory)
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
                    UniqueId = drug.UniqueId,
                    UnitName = drug.Unit.Name,
                    Price = drug.Price,
                    CategoryName = drug.DrugCategory.Name,
                    DiscountPrice = drug.DiscountPrice,
                    Description = drug.Description,
                    Slides = drug.DrugAttachments?.Select(x => x.Url).ToList(),
                    Comments = drug.Comments.Select(x => new DrugCommentDTO { Fullname = x.User.FullName, Comment = x.Comment }).ToList(),
                    Properties = drug.Properties,
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
            && x.DrugAttachments.Any(x => x.AttachmentType == AttachmentType.DrugThumbnailImage))
                .Include(x => x.DrugAttachments)
                .Include(x => x.Unit)
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
                ThumbnailImageUrl = p.DrugAttachments.First(x => x.AttachmentType == AttachmentType.DrugThumbnailImage).Url,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice
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
                    TotalCount = result.Count(),
                    Items = result.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList()
                }
            };
        }

        //public Response<List<GetDrugPriceList>> GetPrices(List<int> ids)
        //{
        //    var result = _Drug.AsNoTracking()
        //        .Include(x => x.DrugPrices)
        //        .ThenInclude(x => x.Unit)
        //        .Where(x => ids.Contains(x.DrugId))
        //        .Select(x => new
        //        {
        //            x.DrugId,
        //            DrugPrices = x.DrugPrices.Select(p => new DrugPriceDTO
        //            {
        //                DrugPriceId = p.DrugPriceId,
        //                IsDefault = p.IsDefault,
        //                Name = p.Unit.Name,
        //                Price = p.Price,
        //                DiscountPrice = p.DiscountPrice,

        //            })
        //        }).ToList();
        //    return new Response<List<GetDrugPriceList>>
        //    {
        //        IsSuccessful = true,
        //        Result = result.Select(x => new GetDrugPriceList
        //        {
        //            DrugId = x.DrugId,
        //            DrugPrices = x.DrugPrices.ToList()
        //        }).ToList()
        //    };
        //}

        //public Response<List<DrugPriceDTO>> GetSingleDrugPrice(int id)
        //{
        //    var items = _Drug.AsNoTracking().Where(x => x.DrugId == id)
        //    .Include(x => x.DrugPrices)
        //    .ThenInclude(x => x.Unit)
        //    .Select(x => x.DrugPrices.Select(p => new DrugPriceDTO
        //    {
        //        DrugPriceId = p.DrugPriceId,
        //        IsDefault = p.IsDefault,
        //        Name = p.Unit.Name,
        //        Price = p.Price,
        //        DiscountPrice = p.DiscountPrice,
        //    })).FirstOrDefault();
        //    if (items == null) return new Response<List<DrugPriceDTO>> { Message = Strings.ItemNotFound };
        //    return new Response<List<DrugPriceDTO>> { IsSuccessful = true, Result = items.ToList() };
        //}
    }
}
