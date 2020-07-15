using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(DrugPrice), Schema = "Drug")]
    public class DrugPrice : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugPriceId { get; set; }

        [Display(Name = nameof(Strings.Unit), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int UnitId { get; set; }

        [Display(Name = nameof(Strings.Unit), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int DrugId { get; set; }

        [Display(Name = nameof(Strings.Price), ResourceType = typeof(Strings))]
        public int Price { get; set; }

        [Display(Name = nameof(Strings.DiscountPercent), ResourceType = typeof(Strings))]
        public int DiscountPrice { get; set; }

        [Display(Name = nameof(Strings.IsDefault), ResourceType = typeof(Strings))]
        public bool IsDefault { get; set; }

        [ForeignKey(nameof(DrugId))]
        public Drug Drug { get; set; }

        [ForeignKey(nameof(UnitId))]
        public Unit Unit { get; set; }
    }
}