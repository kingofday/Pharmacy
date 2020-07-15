using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class OrderResult
    {
        public int OrderId { get; set; }
        public int Price { get; set; }
        public int Delay { get; set; }
        public bool Has_Return { get; set; }
        public bool Cashed { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public bool PayAtDestination { get; set; }
        public string OrderToken { get; set; }
        public string OrderDiscount { get; set; }
        public string ExtraParams { get; set; }
        public List<AloPeikAddress> Addresses { get; set; }
    }
}
