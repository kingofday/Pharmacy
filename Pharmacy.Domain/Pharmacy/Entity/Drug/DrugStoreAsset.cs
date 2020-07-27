using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(DrugStoreAsset), Schema = "Drug")]
    public class DrugStoreAsset :BaseAttachment, IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugStoreAssetId { get; set; }

        [Display(Name = nameof(Strings.Pharmacy), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int DrugStoreId { get; set; }

        [ForeignKey(nameof(DrugStoreId))]
        [Display(Name = nameof(Strings.Pharmacy), ResourceType = typeof(Strings))]
        public DrugStore DrugStore { get; set; }

    }
}