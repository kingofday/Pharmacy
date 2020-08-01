using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class GetDrugPriceList
    {
        public int DrugId { get; set; }

        public List<DrugPriceDTO> DrugPrices { get; set; }
    }
}
