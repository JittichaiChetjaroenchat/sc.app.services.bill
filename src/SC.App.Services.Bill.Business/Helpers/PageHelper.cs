using System;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class PageHelper
    {
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;

        public static int GetPage(int page)
        {
            return page <= 0 ? DefaultPage : page;
        }

        public static int GetPageSize(int size)
        {
            return size <= 0 ? DefaultPageSize : size;
        }

        public static int GetPages(int total, int size)
        {
            var pageSize = GetPageSize(size);

            return (int)Math.Ceiling(total / (pageSize * 1.0));
        }
    }
}