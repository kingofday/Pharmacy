namespace Pharmacy.Domain
{
    public class VerifyRequest
    {
        public string ApiKey { get; set; }
        public int OrderId { get; set; }
        public string TransactionId { get; set; }
        public string Url { get; set; }
    }
}
