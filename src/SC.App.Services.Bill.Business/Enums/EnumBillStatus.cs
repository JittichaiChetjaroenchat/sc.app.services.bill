using System.ComponentModel;

namespace SC.App.Services.Bill.Business.Enums
{
    public enum EnumBillStatus
    {
        [Description("unknown")]
        Unknown,

        [Description("pending")]
        Pending,

        [Description("notified")]
        Notified,

        [Description("rejected")]
        Rejected,

        [Description("confirmed")]
        Confirmed,

        [Description("cancelled")]
        Cancelled,

        [Description("done")]
        Done,

        [Description("archived")]
        Archived,

        [Description("deleted")]
        Deleted,

        [Description("deposited")]
        Deposited,

        [Description("cod")]
        Cod
    }
}