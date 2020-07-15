namespace Pharmacy.Domain
{
    public class HillaPayGetTransResponse
    {
        public HillaPayStatus status { get; set; }

        public HillaPayTransResult result_transaction_send { get; set; }
    }


    public class HillaPayTransResult
    {
        public string transaction_id { get; set; }

        public string transaction_url { get; set; }

        public HillaPayAmount amount { get; set; }
    }
}