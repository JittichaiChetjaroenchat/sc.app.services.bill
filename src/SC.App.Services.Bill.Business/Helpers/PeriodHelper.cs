using System;
using SC.App.Services.Bill.Business.Enums;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class PeriodHelper
    {
        public static DateTime GetBegin(EnumPeriod period)
        {
            switch (period)
            {
                case EnumPeriod.Recent:
                    return DateTime.Now;
                case EnumPeriod.LastWeek:
                    return DateTime.Now.AddDays(-7);
                case EnumPeriod.Last15Days:
                    return DateTime.Now.AddDays(-15);
                case EnumPeriod.LastMonth:
                    return DateTime.Now.AddDays(-30);
                case EnumPeriod.Last3Month:
                    return DateTime.Now.AddDays(-90);
                case EnumPeriod.Last6Month:
                    return DateTime.Now.AddDays(-180);
                case EnumPeriod.LastYear:
                    return DateTime.Now.AddDays(-365);
                default:
                    return DateTime.Now;
            }
        }

        public static DateTime GetEnd(EnumPeriod period)
        {
            switch (period)
            {
                case EnumPeriod.Recent:
                case EnumPeriod.LastWeek:
                case EnumPeriod.Last15Days:
                case EnumPeriod.LastMonth:
                case EnumPeriod.Last3Month:
                case EnumPeriod.Last6Month:
                case EnumPeriod.LastYear:
                    return DateTime.Now;
                default:
                    return DateTime.Now;
            }
        }

        public static DateTime GetBegin(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }

        public static DateTime GetEnd(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }
    }
}