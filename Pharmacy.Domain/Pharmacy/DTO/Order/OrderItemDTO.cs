namespace Pharmacy.Domain
{
    public class OrderItemDTO
    {
        public int DrugId { get; set; }
        public int Discount { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public int GetRealPrice() => Price - Discount;
        public int GetTotalPrice() => GetRealPrice() * Count;
    }
}
