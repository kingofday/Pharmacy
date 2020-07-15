using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class AloPeikOrderResult
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Delay { get; set; }
        public int Final_Price { get; set; }
        public bool Has_Return { get; set; }
        public bool Cashed { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public bool Pay_At_Dest { get; set; }
        public string Order_Token { get; set; }
        public string Order_Discount { get; set; }
        //public string Extra_Param { get; set; }
        public List<AloPeikAddress> Addresses { get; set; }
    }
}