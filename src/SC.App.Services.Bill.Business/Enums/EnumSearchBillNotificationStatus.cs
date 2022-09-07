using System.ComponentModel;

namespace SC.App.Services.Bill.Business.Enums
{
    public enum EnumSearchBillNotificationStatus
    {
        [Description("unknown")]
        Unknown,

        [Description("sent_summary")]
        SentSummary,

        [Description("unsent_summary")]
        UnsentSummary
    }
}