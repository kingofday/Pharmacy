using System;

namespace Pharmacy.Domain
{
    public class Store_OrderUpdateModel
    {
        public Guid OrderId { get; set; }

        public OrderDrugStoreStatus OrderDrugStoreStatus { get; set; }

        public string OrderDrugStoreComment { get; set; }
    }
}
