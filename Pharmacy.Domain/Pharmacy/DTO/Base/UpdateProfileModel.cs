using Pharmacy.Domain.Resource;
using System;
using System.ComponentModel.DataAnnotations;


namespace Pharmacy.Domain
{
    public class UpdateProfileModel
    {
        public Guid UserId { get; set; }

        [Display(Name = nameof(Strings.FullName), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string FullName { get; set; }
        [Display(Name = nameof(Strings.Email), ResourceType = typeof(Strings))]
        [EmailAddress(ErrorMessageResourceName = nameof(ErrorMessage.WrongEmailFormat), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Email { get; set; }
        [Display(Name = nameof(Strings.Password), ResourceType = typeof(Strings))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string NewPassword { get; set; }


        [Display(Name = nameof(Strings.IWantToChangePassword), ResourceType = typeof(Strings))]
        public bool ChangePassword { get; set; }
    }
}
