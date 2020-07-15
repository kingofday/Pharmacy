using Elk.Core;

namespace Pharmacy.Domain
{
    public interface IStoreRepo : IGenericRepo<DrugStore>, IScopedInjection
    {
    }
}
