using Pharmacy.Domain.Resource;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class DrugStoreAdminModel
    {
        public int DrugStoreId { get; set; }

        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [MaxLength(40, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Name { get; set; }

        [Display(Name = nameof(Strings.StoreStatus), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        public DrugStoreStatus Status { get; set; }

        [Display(Name = nameof(Strings.IsActive), ResourceType = typeof(Strings))]
        public bool IsActive { get; set; }

        public IFormFile Logo { get; set; }

        public string AppDir { get; set; }

        [NotMapped]
        public DrugStoreAddress Address { get; set; }

        public List<DrugStoreAttachment> Attachments { get; set; }
    }
}
