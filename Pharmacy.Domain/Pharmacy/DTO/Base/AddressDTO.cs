namespace Pharmacy.Domain
{
    public class AddressDTO : LocationDTO
    {
        public int? Id { get; set; }
        public string Fullname { get; set; }
        public string MobileNumber { get; set; }
        public string Details { get; set; }
    }
}
