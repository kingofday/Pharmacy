using System;

namespace Pharmacy.Domain
{
    public class CreateTransactionRequest
    {
        public PaymentType PaymentType { get; set; }
        public int GatewayId { get; set; }
        public string ApiKey { get; set; }
        public Guid OrderId { get; set; }
        public string Url { get; set; }
        public string CallbackUrl { get; set; }
        public int Amount { get; set; }
        public string MobileNumber { get; set; }
        public int Description { get; set; }

    }
}
