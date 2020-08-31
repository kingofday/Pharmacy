using Elk.Core;

namespace Pharmacy.Domain
{
    public class PrescriptionSearchFilter : PagingParameter
    {
        public int? PrescriptionId { get; set; }

        public string MobileNumber { get; set; }
    }
}
