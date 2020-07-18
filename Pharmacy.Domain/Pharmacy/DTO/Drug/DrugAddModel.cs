using Microsoft.AspNetCore.Http;
using Pharmacy.Domain.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [NotMapped]
    public class DrugAddModel//: Drug
    {
        public int DrugId { get; set; }

        [Display(Name = nameof(Strings.DrugCategory), ResourceType = typeof(Strings))]
        public int? DrugCategoryId { get; set; }

        [Display(Name = nameof(Strings.MaxOrderCount), ResourceType = typeof(Strings))]
        public int MaxOrderCount { get; set; }

        [Display(Name = nameof(Strings.IsActive), ResourceType = typeof(Strings))]
        public bool IsActive { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [MaxLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NameFa { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [MaxLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NameEn { get; set; }

        [Display(Name = nameof(Strings.Description), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(1000, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(1000, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Description { get; set; }

        [NotMapped]
        public IList<IFormFile> Files { get; set; }

        [NotMapped]
        public string Root { get; set; }

        [NotMapped]
        public string BaseDomain { get; set; }

        public List<int> TagIds { get; set; }

        public List<DrugPrice> Prices { get; set; }
    }
}
