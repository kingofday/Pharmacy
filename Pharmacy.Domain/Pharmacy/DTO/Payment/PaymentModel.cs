using Elk.Core;
using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class PaymentModel
    {
        [Display(Name = nameof(Strings.TotalPrice), ResourceType = typeof(Strings))]
        public int TotalPrice { get; set; }
        public PagingListDetails<Payment> PagedList { get; set; }
    }
}
