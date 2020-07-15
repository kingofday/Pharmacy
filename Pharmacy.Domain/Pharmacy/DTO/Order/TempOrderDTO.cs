using System;

namespace Pharmacy.Domain
{
    public class TempOrderDTO
    {
        public Guid BasketId { get; set; }
        public int? OrderId { get; set; }
        public int GatewayId { get; set; }
        public int DeliveryId { get; set; }
        public AddressDTO Address { get; set; }
        public Guid UserToken { get; set; }
        public string Description { get; set; }
        public string Reciever { get; set; }
        public string RecieverMobileNumber { get; set; }
    }
}
