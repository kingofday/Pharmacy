using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(PrescriptionAttachment), Schema = "Order")]
    public class PrescriptionAttachment :BaseAttachment, IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionAttachmentId { get; set; }

        [Display(Name = nameof(Strings.Drug), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int PrescriptionId { get; set; }

        [ForeignKey(nameof(PrescriptionId))]
        [Display(Name = nameof(Strings.Prescription), ResourceType = typeof(Strings))]
        public Prescription Prescription { get; set; }

    }
}