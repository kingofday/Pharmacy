namespace Pharmacy.Domain
{
    public class DrugPriceDTO
    {
        public int DrugPriceId { get; set; }
        public string Name { get; set; }
        public int DiscountPrice { get; set; }
        public int Price { get; set; }
        public bool IsDefault { get; set; }
    }
}
