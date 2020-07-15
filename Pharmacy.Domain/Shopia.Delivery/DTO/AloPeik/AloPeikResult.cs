namespace Pharmacy.Domain
{
    public class AloPeikResult<T>
    {
        public string Status { get; set; }
        public T Object { get; set; }
    }
}
