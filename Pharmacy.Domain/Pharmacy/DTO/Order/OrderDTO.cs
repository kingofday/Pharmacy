using System;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class OrderDTO
    {
        public int? OrderId { get; set; }
        public int GatewayId { get; set; }
        public int DeliveryId { get; set; }       
        public AddressDTO Address { get; set; }
        public Guid UserToken { get; set; }
        public string Description { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }
}
