namespace Pharmacy.Domain
{
    public class GetDeliveryPriceDTO
    {
        public  long UniqueId { get; set; }

        public int Price { get; set; }

        public DeliveryType Type { get; set; }
    }
}
