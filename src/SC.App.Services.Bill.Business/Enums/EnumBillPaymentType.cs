using System.ComponentModel;

namespace SC.App.Services.Bill.Business.Enums
{
    public enum EnumBillPaymentType
    {
        [Description("unknown")]
        Unknown,

        [Description("prepaid")]
        PrePaid,

        [Description("postpaid")]
        PostPaid
    }
}