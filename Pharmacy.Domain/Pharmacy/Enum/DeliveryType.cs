using System;
using System.ComponentModel;

namespace Pharmacy.Domain
{
    public enum DeliveryType : byte
    {
        [Description("پیک")]
        Peyk = 0,
        [Description("پست")]
        Post = 1,
    }
}
