using Pharmacy.Domain.Resource;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class TempBasketItemModel
    {
        [Display(Name = nameof(Strings.InsertDate), ResourceType = typeof(Strings))]
        public string InsertDateSh { get; set; }
        [Display(Name = nameof(Strings.BasketId), ResourceType = typeof(Strings))]
        public Guid BasketId { get; set; }
        [Display(Name = nameof(Strings.TotalPrice), ResourceType = typeof(Strings))]
        public int TotalPrice { get; set; }
    }
}
