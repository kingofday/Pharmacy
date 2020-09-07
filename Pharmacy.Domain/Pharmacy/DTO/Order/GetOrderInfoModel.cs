using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class GetOrderInfoModel
    {
        public long UniqueId { get; set; }
        public OrderStatus Status { get; set; }
        public string InsertDateSh { get; set; }
        public int TotalPrice { get; set; }
        public List<GetOrderItemInfoModel> Items { get; set; }
    }
}
