namespace Pharmacy.Domain
{
    public class DrugDTO
    {
        public int DrugId { get; set; }
        public string NameFa { get; set; }
        public string NameEn { get; set; }
        public string UnitName { get; set; }
        public int DiscountPrice { get; set; }
        public int Price { get; set; }
        public string TumbImageUrl { get; set; }
    }
}
