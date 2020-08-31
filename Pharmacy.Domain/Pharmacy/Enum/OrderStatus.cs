using System;
using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum OrderStatus : int
    {
        [Description("لغو شده")]
        Canceled = -2,

        [Description("عدم پذیرش")]
        NotAccepted = -1,

        [Description("در انتظار پرداخت")]
        WaitForPayment = 0,

        [Description("در حال بررسی")]
        InProcessing = 6,

        [Description("پذیرش شده")]
        Accepted= 9,

        [Description("آماده تحویل")]
        WaitForDelivery = 12,

        [Description("موفق")]
        Done = 15,
    }
}
