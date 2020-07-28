using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class DrugSearchFilter : PagingParameter
    {

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        public string Name { get; set; }
        public DrugFilterType Type { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
    }
}
