using System;
using Microsoft.AspNetCore.Http;
using Pharmacy.Domain.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class AddPrescriptionModel
    {
        public Guid? UserId { get; set; }

        public string AppDir { get; set; }

        public List<IFormFile> Files { get; set; }

        [RegularExpression(@"^0?9\d{9}$", ErrorMessageResourceName = nameof(ErrorMessage.InvalidMobileNumber), ErrorMessageResourceType = typeof(ErrorMessage))]
        [Display(Name = nameof(Strings.MobileNumber), ResourceType = typeof(Strings))]
        public string MobileNumber { get; set; }
    }
}
