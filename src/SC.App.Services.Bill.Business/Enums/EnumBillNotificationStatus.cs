using System.ComponentModel;

namespace SC.App.Services.Bill.Business.Enums
{
    public enum EnumBillNotificationStatus
    {
        [Description("unknown")]
        Unknown,

        [Description("success")]
        Success,

        [Description("warning")]
        Warning,

        [Description("error")]
        Error
    }
}