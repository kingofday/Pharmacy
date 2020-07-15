using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(DrugAsset), Schema = "Drug")]
    public class DrugAsset :BaseAttachment, IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugAssetId { get; set; }

        [Display(Name = nameof(Strings.Drug), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int DrugId { get; set; }

        [ForeignKey(nameof(DrugId))]
        [Display(Name = nameof(Strings.Drug), ResourceType = typeof(Strings))]
        public Drug Drug { get; set; }

    }
}