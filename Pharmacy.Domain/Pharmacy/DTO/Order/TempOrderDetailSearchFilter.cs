using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class TempBasketItemSearchFilter : PagingParameter
    {
        [Display(Name = nameof(Strings.BasketId), ResourceType = typeof(Strings))]
        public string BasketId { get; set; }
        [Display(Name = nameof(Strings.FromDate), ResourceType = typeof(Strings))]
        public string FromDateSh { get; set; }
        [Display(Name = nameof(Strings.ToDate), ResourceType = typeof(Strings))]
        public string ToDateSh { get; set; }
    }
}
