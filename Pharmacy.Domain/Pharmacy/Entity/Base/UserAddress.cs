using System;
using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(UserAddress), Schema = "Base")]
    public class UserAddress : BaseAddress, IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAddressId { get; set; }

        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public Guid UserId { get; set; }

        [Display(Name = nameof(Strings.IsDefault), ResourceType = typeof(Strings))]
        public bool IsDefault { get; set; }

        [Display(Name = nameof(Strings.ReciverName), ResourceType = typeof(Strings))]
        public string Fullname { get; set; }

        [Display(Name = nameof(Strings.MobileNumber), ResourceType = typeof(Strings))]
        public long MobileNumber { get; set; }

        [ForeignKey(nameof(UserId))]
        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        public User User { get; set; }
    }
}
