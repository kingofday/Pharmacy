using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum PaymentType : byte
    {
        [Description("سفارش")]
        Order = 0,
        [Description("هزینه ارسال")]
        DeliveryPrice = 1
    }
}
