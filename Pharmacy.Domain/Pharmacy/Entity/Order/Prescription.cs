﻿using System;
using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    [Table(nameof(Prescription), Schema = "Order")]
    public class Prescription: IEntity,IInsertDateProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = nameof(Strings.Identifier), ResourceType = typeof(Strings))]
        public int PrescriptionId { get; set; }

        [Display(Name = nameof(Strings.Status), ResourceType = typeof(Strings))]
        public PrescriptionStatus Status { get; set; }

        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        public DateTime InsertDateMi { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        
        [Column(TypeName = "char(10)")]
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        [MaxLength(10, ErrorMessageResourceName = nameof(ErrorMessage.MaxLength), ErrorMessageResourceType = typeof(ErrorMessage))]
        public string InsertDateSh { get; set; }

        public List<PrescriptionAttachment> Attachments { get; set; }

        public List<PrescriptionItem> Items { get; set; }

        public Order Order { get; set; }
    }
}
