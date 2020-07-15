using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class SingleDrugDTO
    {
        public int DrugId { get; set; }
        public string NameFa { get; set; }
        public string NameEn { get; set; }
        public IList<string> Slides { get; set; }
        public IList<DrugPriceDTO> Prices { get; set; }
        public IList<DrugTagDTO> Tags { get; set; }

    }
}
