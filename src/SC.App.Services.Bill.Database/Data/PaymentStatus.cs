namespace SC.App.Services.Bill.Database.Data
{
    public class PaymentStatus
    {
        public class Pending
        {
            public const string Code = "pending";

            public const string Description = "Pending";
        }

        public class Rejected
        {
            public const string Code = "rejected";

            public const string Description = "Rejected";
        }

        public class Accepted
        {
            public const string Code = "accepted";

            public const string Description = "Accepted";
        }
    }
}