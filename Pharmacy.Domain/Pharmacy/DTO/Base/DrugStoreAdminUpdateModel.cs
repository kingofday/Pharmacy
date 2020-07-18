using Pharmacy.Domain.Resource;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    public class DrugStoreAdminUpdateModel
    {
        public int DrugStoreId { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [MaxLength(40, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Name { get; set; }

        [Display(Name = nameof(Strings.StoreStatus), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public DrugStoreStatus DrugStoreStatus { get; set; }

        [Display(Name = nameof(Strings.IsActive), ResourceType = typeof(Strings))]
        public bool IsActive { get; set; }

        public IFormFile Logo { get; set; }
        public string Root { get; set; }
        public string BaseDomain { get; set; }

        [NotMapped]
        public DrugStoreAddress Address { get; set; }
    }
}
