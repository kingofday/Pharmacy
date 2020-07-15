using System;
using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class DrugStoreSearchFilter : PagingParameter
    {
        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        public Guid?UserId { get; set; }
        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        public string Name { get; set; }
    }
}
