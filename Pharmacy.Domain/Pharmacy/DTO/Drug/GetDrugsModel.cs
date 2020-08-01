using Elk.Core;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class GetDrugsModel
    {
        public int MaxPrice { get; set; }
        public int TotalCount { get; set; }
        public List<DrugDTO> Items { get; set; }
    }
}
