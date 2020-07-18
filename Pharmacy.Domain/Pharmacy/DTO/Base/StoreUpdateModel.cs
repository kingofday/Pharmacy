using Pharmacy.Domain.Resource;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    public class StoreUpdateModel
    {
        public int StoreId { get; set; }

        [Display(Name = nameof(Strings.IsActive), ResourceType = typeof(Strings))]
        public bool IsActive { get; set; }
        public string Root { get; set; }
        public IFormFile Logo { get; set; }
        public string BaseDomain { get; set; }

        [NotMapped]
        public DrugStoreAddress Address { get; set; }
    }
}
