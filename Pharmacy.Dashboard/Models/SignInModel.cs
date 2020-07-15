using Pharmacy.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Dashboard
{

    public class SignInModel
    {
        [RegularExpression(@"^0?9\d{9}$", ErrorMessageResourceName = nameof(Strings.InvalidMobileNumber), ErrorMessageResourceType = typeof(Strings))]
        [Display(Name = nameof(Strings.MobileNumber), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings), AllowEmptyStrings = false)]
        public string MobileNumber { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessageResourceName = nameof(Strings.MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [MinLength(5, ErrorMessageResourceName = nameof(Strings.MinLength), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(20, MinimumLength = 5, ErrorMessageResourceName = nameof(Strings.Min5MaxLength), ErrorMessageResourceType = typeof(Strings))]
        [Display(Name = nameof(Password), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings), AllowEmptyStrings = false)]
        public string Password { get; set; }

        [Display(Name = nameof(Strings.RememberMe), ResourceType = typeof(Strings))]
        public bool RememberMe { get; set; }
    }
}