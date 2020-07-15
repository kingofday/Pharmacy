using System;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Notifier.Service
{
    public class SendEmailStrategy : ISendStrategy
    {
        public Task SendAsync(Notification notification, INotificationRepo notifierUnitOfWork)
        {
            throw new NotImplementedException();
        }
    }
}
