using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class TagSearchFilter : PagingParameter
    {
        [Display(Name = nameof(Strings.Title), ResourceType = typeof(Strings))]
        public string TitleF { get; set; }
    }
}
