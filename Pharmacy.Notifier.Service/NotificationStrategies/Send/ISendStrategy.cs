using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Notifier.Service
{
    public interface ISendStrategy
    {
        Task SendAsync(Notification notification, INotificationRepo notificationRepo);
    }
}
