﻿using Elk.Core;
using Pharmacy.Domain.Resource;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(OrderDrugStore), Schema = "Order")]
    public class OrderDrugStore : IEntity, IInsertDateProperties, IModifyDateProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDrugStoreId { get; set; }

        [Display(Name = nameof(Strings.Status), ResourceType = typeof(Strings))]
        public OrderDrugStoreStatus Status { get; set; }

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

        public Guid OrderId { get; set; }

        public int DrugStoreId { get; set; }

        [Display(Name = nameof(Strings.DeliveryPrice), ResourceType = typeof(Strings))]
        public int DeliveryPrice { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [DataType(DataType.MultilineText)]
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        [MaxLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Comment { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        [ForeignKey(nameof(DrugStoreId))]
        public DrugStore DrugStore { get; set; }
    }
}
