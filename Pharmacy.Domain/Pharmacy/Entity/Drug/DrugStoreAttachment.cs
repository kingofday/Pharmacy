using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(DrugStoreAttachment), Schema = "Drug")]
    public class DrugStoreAttachment : BaseAttachment, IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugStoreAttachmentId { get; set; }

        [Display(Name = nameof(Strings.Pharmacy), ResourceType = typeof(Strings))]
        public int DrugStoreId { get; set; }

        [ForeignKey(nameof(DrugStoreId))]
        [Display(Name = nameof(Strings.Pharmacy), ResourceType = typeof(Strings))]
        public DrugStore DrugStore { get; set; }

    }
}