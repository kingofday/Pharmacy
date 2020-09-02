using Pharmacy.Domain;
using System;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class AddOrderReponse
    {
        public Guid OrderId { get; set; }
        public string Url { get; set; }
        public bool BasketChanged { get; set; }
        public IEnumerable<DrugDTO> Drugs { get; set; }
}
}
