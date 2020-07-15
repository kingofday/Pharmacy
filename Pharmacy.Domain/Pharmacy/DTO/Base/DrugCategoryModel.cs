namespace Pharmacy.Domain
{
    public class DrugCategoryModel
    {
        public int ItemId { get; set; }
        public int ParentId { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }
    }
}
