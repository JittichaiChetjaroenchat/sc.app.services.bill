using System.ComponentModel;

namespace SC.App.Services.Bill.Business.Enums
{
    public enum EnumOrderStatus
    {
        [Description("unknown")]
        Unknown,

        [Description("pending")]
        Pending,

        [Description("confirm")]
        Confirm,

        [Description("cancelled")]
        Cancelled
    }
}