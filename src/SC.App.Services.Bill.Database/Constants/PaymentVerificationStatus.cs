namespace SC.App.Services.Bill.Database.Constants
{
    public class PaymentVerificationStatus
    {
        public const string TableName = "payment_verification_statuses";

        public static class Column
        {
            public const string Id = "id";

            public const string Code = "code";

            public const string Description = "description";

            public const string Index = "index";
        }
    }
}