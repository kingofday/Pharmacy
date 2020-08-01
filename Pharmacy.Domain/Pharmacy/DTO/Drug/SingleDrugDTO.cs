using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class SingleDrugDTO
    {
        public int DrugId { get; set; }
        public string NameFa { get; set; }
        public string NameEn { get; set; }
        public string CategoryName { get; set; }
        public int Price { get; set; }
        public int DiscountPrice { get; set; }
        public string UniqueId { get; set; }
        public string UnitName { get; set; }
        public IList<string> Slides { get; set; }
        public IList<DrugTagDTO> Tags { get; set; }

    }
}
