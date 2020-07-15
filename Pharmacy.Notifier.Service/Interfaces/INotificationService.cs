using Elk.Core;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Notifier.Service
{
    public interface INotificationService
    {
        Task<IResponse<bool>> AddAsync(NotificationDto notifyDto, int applicationId);
        
        Task SendAsync();
    }
}
