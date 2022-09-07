using System.ComponentModel;

namespace SC.App.Services.Bill.Business.Enums
{
    public enum EnumParcelStatus
    {
        [Description("unknown")]
        Unknown,

        [Description("active")]
        Active,

        [Description("cancelled")]
        Cancelled
    }
}