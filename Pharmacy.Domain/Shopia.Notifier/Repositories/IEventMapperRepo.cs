using Elk.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pharmacy.Domain
{
    public interface IEventMapperRepo : IScopedInjection
    {
        Task<bool> AddAsync(EventMapper model);
        Task<IEnumerable<EventMapper>> GetAsync(EventType eventType, int applicationId);
    }
}
