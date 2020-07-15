using Elk.Core;
using System.Threading.Tasks;

namespace Pharmacy.Domain
{
    public interface IUserRepo : IGenericRepo<User>, IScopedInjection
    {}
}
