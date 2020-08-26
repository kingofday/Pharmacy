using Pharmacy.Domain.Resource;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Domain
{
    public class LocationDTO
    {
        [Display(Name = nameof(Strings.Latitude), ResourceType = typeof(Strings))]
        public double Latitude { get; set; } = 35.699858;

        [Display(Name = nameof(Strings.Longitude), ResourceType = typeof(Strings))]
        public double Longitude { get; set; } = 51.337848;
    }
}
