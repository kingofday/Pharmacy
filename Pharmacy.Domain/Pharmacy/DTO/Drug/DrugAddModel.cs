using Microsoft.AspNetCore.Http;
using Pharmacy.Domain.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [NotMapped]
    public class DrugAddModel
    {
        public int DrugId { get; set; }

        public int Price { get; set; }

        public int DiscountPrice { get; set; }

        public int UnitId { get; set; }

        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [Display(Name = nameof(Strings.UniqueId), ResourceType = typeof(Strings))]
        public string UniqueId { get; set; }


        [Display(Name = nameof(Strings.DrugCategory), ResourceType = typeof(Strings))]
        public int? DrugCategoryId { get; set; }

        [Display(Name = nameof(Strings.MaxOrderCount), ResourceType = typeof(Strings))]
        public int MaxOrderCount { get; set; }

        [Display(Name = nameof(Strings.IsActive), ResourceType = typeof(Strings))]
        public bool IsActive { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NameFa { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(35, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NameEn { get; set; }

        [Display(Name = nameof(Strings.ShortDescription), ResourceType = typeof(Strings))]
        public string ShortDescription { get; set; }

        [Display(Name = nameof(Strings.Description), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(1000, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(1000, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Description { get; set; }

        public IList<IFormFile> Files { get; set; }

        public List<int> TagIds { get; set; }

        public string AppDir { get; set; }

        public List<DrugProperty> Properties { get; set; }

    }
}
