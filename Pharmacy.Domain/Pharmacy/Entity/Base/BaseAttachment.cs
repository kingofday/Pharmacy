using Elk.Core;
using System;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    public abstract class BaseAttachment
    {
        [Display(Name = nameof(Strings.FileType), ResourceType = typeof(Strings))]
        public FileType FileType { get; set; }

        [Display(Name = nameof(Strings.Type), ResourceType = typeof(Strings))]
        public AttachmentType AttachmentType { get; set; }

        [Display(Name = nameof(Strings.Size), ResourceType = typeof(Strings))]
        public int Size { get; set; }

        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        public DateTime InsertDateMi { get; set; }

        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        [Column(TypeName = "varchar(5)")]
        [Display(Name = nameof(Strings.Extention), ResourceType = typeof(Strings))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessage.Required), ErrorMessageResourceType = typeof(ErrorMessage))]
        [MaxLength(5, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Extention { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string OriginalName { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(50, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string FileName { get; set; }

        [Column(TypeName = "varchar(250)")]
        [Display(Name = nameof(Strings.CdnFileUrl), ResourceType = typeof(Strings))]
        [MaxLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string Url { get; set; }

        [Column(TypeName = "varchar(250)")]
        [Display(Name = nameof(Strings.CdnFileUrl), ResourceType = typeof(Strings))]
        [MaxLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        [StringLength(250, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string PhysicalPath { get; set; }
    }
}
