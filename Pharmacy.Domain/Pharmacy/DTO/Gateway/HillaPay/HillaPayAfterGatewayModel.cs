namespace Pharmacy.Domain
{
    public class HillaPayAfterGatewayModel
    {
        public HillaPayStatus Status { get; set; }

        public HillaPayAfterGatewayResult result_transaction_callback { get; set; }
    }

    public class HillaPayAfterGatewayResult
    {
        public string transaction_id { get; set; }

        public string rrn { get; set; }

        public HillaPayAmount amount { get; set; }
    }
}
