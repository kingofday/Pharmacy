using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class AloPeikAddressInquiry
    {
        public List<string> Address { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string City_fa { get; set; }
    }
}
