using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum EventType : byte
    {
        [Description("ثبت نام")]
        Subscription = 1,

        [Description("کراول")]
        Crawl = 2,

        [Description("سفارش")]
        Order = 3
    }
}
