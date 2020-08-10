namespace Pharmacy.Domain
{
    public class AddressDTO : LocationDTO
    {
        public int? Id { get; set; }
        public string Details { get; set; }
    }
}
