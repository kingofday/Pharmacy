using System;
using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum OrderStatus : int
    {
        [Description("لغو شده")]
        Canceled = -1,

        [Description("در انتظار پرداخت")]
        WaitForPayment = 0,

        [Description("در حال بررسی")]
        InProcessing = 6,

        [Description("پذیرش شده")]
        Accepted= 9,

        [Description("عدم پذیرش")]
        NotAccepted = 10,

        [Description("در انتظار تحویل")]
        WaitForDelivery = 12,

        [Description("موفق")]
        Done = 15,
    }
}
