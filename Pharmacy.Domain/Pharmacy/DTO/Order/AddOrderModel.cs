namespace Pharmacy.Domain
{
    public class AddOrderModel
    {
        public string ApiKey { get; set; }

        public string TransactionUrl { get; set; } = "https://api.hillapay.ir/ipg/v3/send";

        public string CallbackUrl { get; set; } = "https://localhost:44328/Payment/AfterGateway";
    }
}
