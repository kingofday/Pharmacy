using Pharmacy.Domain;
using System.Collections.Generic;

namespace Pharmacy.Store.Api
{
    public class AddOrderReponse
    {
        public int OrderId { get; set; }
        public string Url { get; set; }
        public bool BasketChanged { get; set; }
        public IEnumerable<DrugDTO> Drugs { get; set; }
}
}
