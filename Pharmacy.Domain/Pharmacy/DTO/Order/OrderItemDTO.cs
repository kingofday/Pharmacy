namespace Pharmacy.Domain
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int PriceId { get; set; }
        public int DiscountPrice { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int GetRealPrice() => Price - DiscountPrice;
        public int GetTotalPrice() => GetRealPrice() * Count;
    }
}
