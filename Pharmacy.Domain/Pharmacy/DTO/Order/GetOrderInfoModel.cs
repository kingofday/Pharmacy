using System;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class GetOrderInfoModel
    {
        public Guid OrderId { get; set; }
        public long UniqueId { get; set; }
        public bool NeedDeliveryPayment { get; set; }
        public string Status { get; set; }
        public string InsertDateSh { get; set; }
        public int TotalPrice { get; set; }
        public List<GetOrderItemInfoModel> Items { get; set; }
    }
}
