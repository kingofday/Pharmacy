using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(DrugStoreAddress), Schema = "Base")]
    public class DrugStoreAddress : BaseAddress, IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugStoreAddressId { get; set; }

        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int DrugStoreId { get; set; }

        [ForeignKey(nameof(DrugStore))]
        [Display(Name = nameof(Strings.Pharmacy), ResourceType = typeof(Strings))]
        public DrugStore DrugStore { get; set; }
    }
}
