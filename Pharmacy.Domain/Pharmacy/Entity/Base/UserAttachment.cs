using System;
using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Domain
{
    [Table(nameof(UserAttachment), Schema = "Base")]
    public class UserAttachment : BaseAttachment, IInsertDateProperties, IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAttachmentId { get; set; }

        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        public User User { get; set; }
    }
}
