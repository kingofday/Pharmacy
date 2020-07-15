﻿using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum NotificationType : byte
    {
        [Description("پیامک")]
        Sms = 1,

        [Description("تلگرام")]
        TeleBot = 2,

        [Description("ایمیل")]
        Email = 3
    }
}
