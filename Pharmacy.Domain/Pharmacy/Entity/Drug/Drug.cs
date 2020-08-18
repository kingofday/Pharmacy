using System;
using Elk.Core;
using Pharmacy.Domain.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(Drug), Schema = "Drug")]
    public class Drug : IInsertDateProperties, IModifyDateProperties, ISoftDeleteProperty, IIsActiveProperty, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugId { get; set; }

        [Display(Name = nameof(Strings.Price), ResourceType = typeof(Strings))]
        public int Price { get; set; }

        [Display(Name = nameof(Strings.DiscountPrice), ResourceType = typeof(Strings))]
        public int DiscountPrice { get; set; }

        [Display(Name = nameof(Strings.UniqueId), ResourceType = typeof(Strings))]
        public int UnitId { get; set; }

        [Display(Name = nameof(Strings.DrugCategory), ResourceType = typeof(Strings))]
        public int? DrugCategoryId { get; set; }

        [Display(Name = nameof(Strings.IsActive), ResourceType = typeof(Strings))]
        public bool IsActive { get; set; }

        [Display(Name = nameof(Strings.NeedPrescription), ResourceType = typeof(Strings))]
        public bool NeedPrescription { get; set; }

        [Display(Name = nameof(Strings.IsDeleted), ResourceType = typeof(Strings))]
        public bool IsDeleted { get; set; }

        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        public DateTime InsertDateMi { get; set; }

        [Display(Name = nameof(Strings.ModifyDate), ResourceType = typeof(Strings))]
        public DateTime ModifyDateMi { get; set; }

        [Display(Name = nameof(Strings.ViewCount), ResourceType = typeof(Strings))]
        public int ViewCount { get; set; }

        [Display(Name = nameof(Strings.LikeCount), ResourceType = typeof(Strings))]
        public int LikeCount { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.ModifyDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string ModifyDateSh { get; set; }

        [Column(TypeName = "varchar(20)")]
        [Display(Name = nameof(Strings.UniqueId), ResourceType = typeof(Strings))]
        [MaxLength(20, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string UniqueId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [MaxLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NameFa { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = nameof(Strings.NameEn), ResourceType = typeof(Strings))]
        [MaxLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(100, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NameEn { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = nameof(Strings.ShortDescription), ResourceType = typeof(Strings))]
        [MaxLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string ShortDescription { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        [DataType(DataType.MultilineText)]
        [Display(Name = nameof(Strings.Description), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(1000, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(1000, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Description { get; set; }

        [ForeignKey(nameof(DrugCategoryId))]
        [Display(Name = nameof(Strings.DrugCategory), ResourceType = typeof(Strings))]
        public DrugCategory DrugCategory { get; set; }

        [ForeignKey(nameof(UnitId))]
        public Unit Unit { get; set; }
        public IList<DrugAsset> DrugAssets { get; set; }
        public IList<OrderItem> OrderDetails { get; set; }

        [Display(Name = nameof(Strings.Tag), ResourceType = typeof(Strings))]
        public IList<DrugTag> DrugTags { get; set; }

        [Display(Name = nameof(Strings.Comments), ResourceType = typeof(Strings))]
        public IList<DrugComment> Comments { get; set; }

        [Display(Name = nameof(Strings.Properies), ResourceType = typeof(Strings))]
        public IList<DrugProperty> Properties { get; set; }
    }
}
