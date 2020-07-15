﻿using System;
using Pharmacy.Domain;
using System.Threading.Tasks;

namespace Pharmacy.Notifier.Service
{
    public class SendTeleBotStrategy : ISendStrategy
    {
        public async Task SendAsync(Notification notification, INotificationRepo notificationRepo)
        {
            await TelegramBot._client.SendTextMessageAsync(notification.Receiver, notification.Content);

            var updateModel = new UpdateNotificationDto
            {
                NotificationId = notification.NotificationId,
                Status = NotificationStatus.Success,
                SendDateMi = DateTime.Now,
                SendStatus = "Success",
                IsLock = true
            };
            await notificationRepo.UpdateAsync(updateModel);
        }
    }
}
