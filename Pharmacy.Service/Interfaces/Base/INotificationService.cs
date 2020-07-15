using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface INotificationService
    {
        Task<IResponse<bool>> NotifyAsync(NotificationDto notifyDto);
    }
}
