using System;
using Elk.Core;
using Pharmacy.Domain.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pharmacy.Domain
{
    [Table(nameof(Order), Schema = "Order")]
    public class Order : IInsertDateProperties, IModifyDateProperties, ISoftDeleteProperty, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = nameof(Strings.Identifier), ResourceType = typeof(Strings))]
        public Guid OrderId { get; set; }

        public bool IsFixed { get; set; }

        public DeliveryType DeliveryType { get; set; }

        [Column(TypeName = "varchar(20)")]
        [MaxLength(20, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(20, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string DeliveryAgentName { get; set; }

        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        //[Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public Guid UserId { get; set; }

        public int? PrescriptionId { get; set; }

        public Guid? TempBasketId { get; set; }

        public int DrugStoreId{ get; set; }

        [Display(Name = nameof(Strings.CustomerAddress), ResourceType = typeof(Strings))]
        public int AddressId { get; set; }

        [Display(Name = nameof(Strings.DeliveryTime), ResourceType = typeof(Strings))]
        public int DeliveryTime { get; set; }

        [Display(Name = nameof(Strings.DiscountPrice), ResourceType = typeof(Strings))]
        public int TotalDiscountPrice { get; set; }


        [Display(Name = nameof(Strings.DeliveryPrice), ResourceType = typeof(Strings))]
        public int DeliveryPrice { get; set; }

        [Display(Name = nameof(Strings.TotalPrice), ResourceType = typeof(Strings))]
        public int TotalItemsPrice { get; set; }

        [Display(Name = nameof(Strings.TotalPriceWithoutDiscount), ResourceType = typeof(Strings))]
        public int TotalPriceWithoutDiscount { get; set; }

        [Display(Name = nameof(Strings.TotalPrice), ResourceType = typeof(Strings))]
        public int TotalPrice { get; set; }

        [Display(Name = nameof(Strings.OrderStatus), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public OrderStatus OrderStatus { get; set; }

        [Display(Name = nameof(Strings.IsDeleted), ResourceType = typeof(Strings))]
        public bool IsDeleted { get; set; }

        [Display(Name = nameof(Strings.PreparationDate), ResourceType = typeof(Strings))]
        public DateTime? PreparationDate { get; set; }

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

        [Display(Name = nameof(Strings.Description), ResourceType = typeof(Strings))]
        [MaxLength(150, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(150, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Comment { get; set; }

        [Display(Name = nameof(Strings.OrderComment), ResourceType = typeof(Strings))]
        [MaxLength(300, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(300, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string ExtraInfoJson { get; set; }

        [Display(Name = nameof(Strings.OrderDetails), ResourceType = typeof(Strings))]
        public List<OrderItem> OrderItems { get; set; }

        [Display(Name = nameof(Strings.Payments), ResourceType = typeof(Strings))]
        public List<Payment> Payments { get; set; }

        [Display(Name = nameof(Strings.Payments), ResourceType = typeof(Strings))]
        public List<OrderDrugStore> OrderDrugStores { get; set; }


        [ForeignKey(nameof(AddressId))]
        [Display(Name = nameof(Strings.CustomerAddress), ResourceType = typeof(Strings))]
        public UserAddress Address { get; set; }


        [ForeignKey(nameof(PrescriptionId))]
        [Display(Name = nameof(Strings.Drug), ResourceType = typeof(Strings))]
        public Prescription Prescription { get; set; }

        [NotMapped]
        [Display(Name = nameof(Strings.Pharmacy), ResourceType = typeof(Strings))]
        public DrugStore DrugStore { get; set; }
    }
}