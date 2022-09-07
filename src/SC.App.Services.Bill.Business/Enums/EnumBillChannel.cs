using System.ComponentModel;

namespace SC.App.Services.Bill.Business.Enums
{
    public enum EnumBillChannel
    {
        [Description("unknown")]
        Unknown,

        [Description("offline")]
        Offline,

        [Description("facebook")]
        Facebook
    }
}