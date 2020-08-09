using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class ConfirmModel
    {
        [RegularExpression(@"^0?9\d{9}$", ErrorMessageResourceName = nameof(ErrorMessage.InvalidMobileNumber), ErrorMessageResourceType = typeof(ErrorMessage))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(11, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string MobileNumber { get; set; }
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Code { get; set; }
    }
}
