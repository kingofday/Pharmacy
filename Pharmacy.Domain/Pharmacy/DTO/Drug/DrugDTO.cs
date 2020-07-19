namespace Pharmacy.Domain
{
    public class DrugDTO
    {
        public int DrugId { get; set; }
        public string UniqueId { get; set; }
        public int PriceId { get; set; }
        public string NameFa { get; set; }
        public string NameEn { get; set; }
        public string UnitName { get; set; }
        public int DiscountPrice { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string TumbnailImageUrl { get; set; }
        public int GetTotalPrice() => (Price - DiscountPrice) * Count;
    }
}
