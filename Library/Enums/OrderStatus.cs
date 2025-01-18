using System.ComponentModel;

namespace Library.Enums
{
    public enum OrderStatus
    {
        [Description("in progress")]
        InProgress,
        [Description("waiting for payment")]
        WaitingForPayment,
        [Description("completed")]
        Completed,
    }
}
