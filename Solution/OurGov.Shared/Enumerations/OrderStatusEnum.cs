using System.ComponentModel;

namespace MeShopAlot.Data.Shared.Enumerations
{
    public enum OrderStatusEnum
    {
        [Description("Payment Failed")]
        PaymentFailed = 1,
        [Description("Payment Received")]
        PaymentReceived = 2,
        [Description("Pending")]
        Pending = 3
    }
}
