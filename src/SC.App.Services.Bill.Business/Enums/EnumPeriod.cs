using System.ComponentModel;

namespace SC.App.Services.Bill.Business.Enums
{
    public enum EnumPeriod
    {
        [Description("unknown")]
        Unknown,

        [Description("recent")]
        Recent,

        [Description("lastweek")]
        LastWeek,

        [Description("last15days")]
        Last15Days,

        [Description("lastmonth")]
        LastMonth,

        [Description("last3month")]
        Last3Month,

        [Description("last6month")]
        Last6Month,

        [Description("lastyear")]
        LastYear,

        [Description("custom")]
        Custom
    }
}