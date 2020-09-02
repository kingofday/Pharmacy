using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;


namespace Pharmacy.Domain
{
    public class SignUpModel
    {
        [Display(Name = nameof(Strings.FullName), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Fullname { get; set; }
        [Display(Name = nameof(Strings.Email), ResourceType = typeof(Strings))]
        [EmailAddress(ErrorMessageResourceName = nameof(ErrorMessage.WrongEmailFormat), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Email { get; set; }
        [Display(Name = nameof(Strings.Password), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NewPassword { get; set; }
        [RegularExpression(@"^0?9\d{9}$", ErrorMessageResourceName = nameof(ErrorMessage.InvalidMobileNumber), ErrorMessageResourceType = typeof(ErrorMessage))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(11, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string MobileNumber { get; set; }
    }
}
