namespace Pharmacy.Domain
{
    public class HillaPayVerifyReponse
    {
        public HillaPayStatus status { get; set; }

        public HillaPayVerifyResult result_transaction_verify { get; set; }
    }

    public class HillaPayVerifyResult
    {
        public string transaction_id { get; set; }
        public long order_id { get; set; }
        public string card { get; set; }
    }
}
