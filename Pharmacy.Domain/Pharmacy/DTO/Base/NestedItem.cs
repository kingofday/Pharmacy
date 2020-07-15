namespace Pharmacy.Domain
{
    public partial class NestedItem
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int OrderPrority { get; set; }
        public string Icon { get; set; }
    }
}
