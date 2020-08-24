using System;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class OrderDTO
    {
        public Guid? OrderId { get; set; }
        public int GatewayId { get; set; }
        public AddressDTO Address { get; set; }
        public DeliveryType DeliveryType { get; set; }       
        public string Comment { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }
}
