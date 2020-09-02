namespace Pharmacy.Domain
{
    public class DeliveryOrderLocationDTO : LocationDTO
    {
        public string Type { get; set; }
        //public double Lat { get; set; }
        //public double Lng { get; set; }
        public string Description { get; set; }
        public string PersonPhone { get; set; }
        public string PersonFullName { get; set; }
    }
}
