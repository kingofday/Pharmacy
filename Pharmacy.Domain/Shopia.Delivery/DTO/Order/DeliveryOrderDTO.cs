using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class DeliveryOrderDTO
    {
        public List<DeliveryOrderLocationDTO> Addresses { get; set; }
        public string ExtraParams { get; set; }
    }
}
