using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Notifier.Service
{
    public interface ICreateStrategy
    {
        Task Create(NotificationDto notifyDto, INotificationRepo notificationRepo, int applicationId);
    }
}