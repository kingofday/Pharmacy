using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(DrugProperty), Schema = "Drug")]
    public class DrugProperty : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugPropertyId { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [MaxLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(25, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = nameof(Strings.Value), ResourceType = typeof(Strings))]
        [MaxLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Value { get; set; }

        [Display(Name = nameof(Strings.Drug), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int DrugId { get; set; }

        [ForeignKey(nameof(DrugId))]
        [Display(Name = nameof(Strings.Drug), ResourceType = typeof(Strings))]
        public Drug Drug { get; set; }

    }
}