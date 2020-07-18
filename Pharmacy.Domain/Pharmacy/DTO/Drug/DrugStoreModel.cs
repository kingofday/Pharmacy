namespace Pharmacy.Domain
{
    public class DrugStoreModel : LocationDTO
    {
        public int DrugStoreId { get; set; }

        public string Name { get; set; }

        public double Distance { get; set; }
    }
}
