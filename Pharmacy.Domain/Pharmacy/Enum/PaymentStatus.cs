using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum PaymentStatus : int
    {
        [Description("ناموفق")]
        Failed = -1,
        [Description("انصراف")]
        Canceled = 0,
        [Description("در انتظار پرداخت")]
        UnProccesed = 1,
        [Description("موفق")]
        Success = 2
    }
}