using System.ComponentModel;

namespace SC.App.Services.Bill.Business.Enums
{
    public enum EnumCourier
    {
        [Description("unknown")]
        Unknown,

        [Description("flash")]
        Flash,

        [Description("jt")]
        JT,

        [Description("tp")]
        ThailandPost,

        [Description("scg")]
        SCG
    }
}