namespace SC.App.Services.Bill.Database.Constants
{
    public class BillPaymentType
    {
        public const string TableName = "bill_payment_types";

        public static class Column
        {
            public const string Id = "id";

            public const string Code = "code";

            public const string Description = "description";

            public const string Index = "index";
        }
    }
}