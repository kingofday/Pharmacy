using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class SignInModel
    {
        [RegularExpression(@"^0?9\d{9}$", ErrorMessageResourceName = nameof(ErrorMessage.InvalidMobileNumber), ErrorMessageResourceType = typeof(ErrorMessage))]
        [Display(Name = nameof(Strings.MobileNumber), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage), AllowEmptyStrings = false)]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MinLength(5, ErrorMessageResourceName = nameof(ErrorMessage.MinLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(20, MinimumLength = 5, ErrorMessageResourceName = nameof(ErrorMessage.LengthRangeError), ErrorMessageResourceType = typeof(ErrorMessage))]
        [Display(Name = nameof(Password), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage), AllowEmptyStrings = false)]
        public string Password { get; set; }

        [Display(Name = nameof(Strings.RememberMe), ResourceType = typeof(Strings))]
        public bool RememberMe { get; set; }
    }
}