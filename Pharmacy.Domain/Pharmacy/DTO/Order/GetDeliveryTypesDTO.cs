using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public class GetDeliveryTypesDTO
    {
        public string PlaceName { get; set; }

        public List<DeliveryDto> Items { get; set; }
    }
}
