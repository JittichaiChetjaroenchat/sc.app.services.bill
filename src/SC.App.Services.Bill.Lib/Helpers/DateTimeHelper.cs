using System;

namespace SC.App.Services.Bill.Lib.Helpers
{
    public class DateTimeHelper
    {
        public static bool IsPresentDate(DateTime value)
        {
            return value.Date == DateTime.Now.Date;
        }

        public static bool IsPresentMonth(DateTime value)
        {
            return value.Year == DateTime.Now.Year && value.Month == DateTime.Now.Month;
        }

        public static bool IsPresentYear(DateTime value)
        {
            return value.Year == DateTime.Now.Year;
        }
    }
}