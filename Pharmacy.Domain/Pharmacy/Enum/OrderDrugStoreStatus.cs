﻿using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum OrderDrugStoreStatus
    {
        [Description("در حال بررسی")]
        InProccessing = 0,
        [Description("قبول سفارش")]
        Accepted = 1,
        [Description("پرداخت شده")]
        Payed = 2,
        [Description("رد سفارش")]
        Denied = 3
    }
}
