using System;
using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(Payment), Schema = "Payment")]
    public class Payment : IInsertDateProperties, IModifyDateProperties, ISoftDeleteProperty, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [ForeignKey(nameof(PaymentGatewayId))]
        [Display(Name = nameof(Strings.PaymentGateway), ResourceType = typeof(Strings))]
        public PaymentGateway PaymentGateway { get; set; }
        [Display(Name = nameof(Strings.PaymentGateway), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int PaymentGatewayId { get; set; }

        [ForeignKey(nameof(OrderId))]
        [Display(Name = nameof(Strings.Order), ResourceType = typeof(Strings))]
        public Order Order { get; set; }
        [Display(Name = nameof(Strings.Order), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int OrderId { get; set; }

        [Display(Name = nameof(Strings.Price), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public int Price { get; set; }

        [Display(Name = nameof(Strings.PaymentStatus), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public PaymentStatus PaymentStatus { get; set; }

        [Display(Name = nameof(Strings.IsDeleted), ResourceType = typeof(Strings))]
        public bool IsDeleted { get; set; }

        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        public DateTime InsertDateMi { get; set; }

        [Display(Name = nameof(Strings.ModifyDate), ResourceType = typeof(Strings))]
        public DateTime ModifyDateMi { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.ModifyDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string ModifyDateSh { get; set; }

        [Display(Name = nameof(Strings.Title), ResourceType = typeof(Strings))]
        [MaxLength(150, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(150, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Title { get; set; }

        [Column(TypeName = "varchar(36)")]
        [Display(Name = nameof(Strings.TransactionId), ResourceType = typeof(Strings))]
        [MaxLength(36, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(36, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string TransactionId { get; set; }
    }
}