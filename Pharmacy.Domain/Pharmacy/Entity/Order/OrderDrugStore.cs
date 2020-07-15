using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(Order), Schema = "Order")]
    public class OrderDrugStore : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDrugStoreId { get; set; }

        public OrderPharmacyStatus Status { get; set; }

        public int OrderId { get; set; }

        public int DrugStoreId { get; set; }

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
