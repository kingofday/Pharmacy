using System;
using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(DrugTag), Schema = "Drug")]
    public class DrugTag : IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugTagId { get; set; }

        [Display(Name = nameof(Strings.Drug), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int DrugId { get; set; }

        [Display(Name = nameof(Strings.Tag), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int TagId { get; set; }

        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        public DateTime InsertDateMi { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [ForeignKey(nameof(TagId))]
        [Display(Name = nameof(Strings.Tag), ResourceType = typeof(Strings))]
        public Tag Tag { get; set; }

        [ForeignKey(nameof(DrugId))]
        [Display(Name = nameof(Strings.Drug), ResourceType = typeof(Strings))]
        public Drug Drug { get; set; }
    }
}