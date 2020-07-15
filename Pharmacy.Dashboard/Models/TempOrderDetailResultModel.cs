using Pharmacy.Dashboard.Resources;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Dashboard
{
    public class TempOrderDetailResultModel
    {
        public string Url { get; set; }

        [RegularExpression(@"^0?9\d{9}$", ErrorMessageResourceName = nameof(Strings.InvalidMobileNumber), ErrorMessageResourceType = typeof(Strings))]
        [Display(Name = nameof(Strings.MobileNumber), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(Strings.Required), ErrorMessageResourceType = typeof(Strings), AllowEmptyStrings = false)]
        public string MobileNumber { get; set; }
    }
}
