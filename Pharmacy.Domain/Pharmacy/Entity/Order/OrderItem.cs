using Elk.Core;
using System;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(OrderItem), Schema = "Order")]
    public class OrderItem : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        [Display(Name = nameof(Strings.Order), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public Guid OrderId { get; set; }

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


        [ForeignKey(nameof(OrderId))]
        [Display(Name = nameof(Strings.Order), ResourceType = typeof(Strings))]
        public Order Order { get; set; }

        [ForeignKey(nameof(DrugId))]
        [Display(Name = nameof(Strings.Order), ResourceType = typeof(Strings))]
        public Drug Drug { get; set; }
    }
}
