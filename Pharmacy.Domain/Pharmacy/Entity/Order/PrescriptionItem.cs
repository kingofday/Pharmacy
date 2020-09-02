using Elk.Core;
using System;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(PrescriptionItem), Schema = "Order")]
    public class PrescriptionItem : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionItemId { get; set; }

        public int PrescriptionId { get; set; }

        [Display(Name = nameof(Strings.Drug), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int DrugId { get; set; }


        [Display(Name = nameof(Strings.Count), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int Count { get; set; }

        [Display(Name = nameof(Strings.Price), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int Price { get; set; }

        [Display(Name = nameof(Strings.Discount), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int DiscountPrice { get; set; }

        [Display(Name = nameof(Strings.TotalPrice), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int TotalPrice { get; set; }

        public int GetRealPrice() => Price - DiscountPrice;

        [ForeignKey(nameof(DrugId))]
        [Display(Name = nameof(Strings.Order), ResourceType = typeof(Strings))]
        public Drug Drug { get; set; }

        [ForeignKey(nameof(PrescriptionId))]
        [Display(Name = nameof(Strings.Prescription), ResourceType = typeof(Strings))]
        public Prescription Prescription { get; set; }
    }
}
