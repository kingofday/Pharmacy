using Elk.Core;
using Pharmacy.Domain.Resource;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class DrugSearchFilter : PagingParameter
    {

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        public string Name { get; set; }
        public DrugFilterCategory Category { get; set; }
    }
}
