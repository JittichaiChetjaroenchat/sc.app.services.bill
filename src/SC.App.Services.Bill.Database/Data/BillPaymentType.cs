namespace SC.App.Services.Bill.Database.Data
{
    public class BillPaymentType
    {
        public class PrePaid
        {
            public const string Code = "prepaid";

            public const string Description = "Pre Paid";
        }

        public class PostPaid
        {
            public const string Code = "postpaid";

            public const string Description = "Post Paid";
        }
    }
}