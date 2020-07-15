using System;
using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum OrderStatus : int
    {
        [Description("ناموفق")]
        Failed = -1,
        [Description("در انتظار پرداخت")]
        WaitForPayment = 3,
        [Description("در حال بررسی")]
        InProcessing = 9,
        [Description("در انتظار تحویل")]
        WaitForDelivery = 12,
        [Description("موفق")]
        Successed = 15,
    }
}
