using System;
using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class OrderSearchFilter : PagingParameter
    {
        [Display(Name = nameof(Strings.User), ResourceType = typeof(Strings))]
        public Guid? UserId { get; set; }
        [Display(Name = nameof(Strings.Pharmacy), ResourceType = typeof(Strings))]
        public int? DrugStoreId { get; set; }
        [Display(Name = nameof(Strings.OrderStatus), ResourceType = typeof(Strings))]
        public OrderStatus? OrderStatus { get; set; }
        [Display(Name = nameof(Strings.TransactionId), ResourceType = typeof(Strings))]
        public string TransactionId { get; set; }
        [Display(Name = nameof(Strings.FromDate), ResourceType = typeof(Strings))]
        public string FromDateSh { get; set; }
        [Display(Name = nameof(Strings.ToDate), ResourceType = typeof(Strings))]
        public string ToDateSh { get; set; }
    }
}
