namespace Pharmacy.Domain
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public float? Discount { get; set; }
        public int DiscountPrice => Count * (Price - RealPrice);
        public int Price { get; set; }
        public int Count { get; set; }
        public int RealPrice { get { return (int)(Price - (Price * (Discount ?? 0) / 100)); } }
    }
}
