namespace SC.App.Services.Bill.Database.Data
{
    public class BillStatus
    {
        public class Pending
        {
            public const string Code = "pending";

            public const string Description = "Pending";
        }

        public class Notified
        {
            public const string Code = "notified";

            public const string Description = "Notified";
        }

        public class Rejected
        {
            public const string Code = "rejected";

            public const string Description = "Rejected";
        }

        public class Confirmed
        {
            public const string Code = "confirmed";

            public const string Description = "Confirmed";
        }

        public class Cancelled
        {
            public const string Code = "cancelled";

            public const string Description = "Cancelled";
        }

        public class Done
        {
            public const string Code = "done";

            public const string Description = "Done";
        }

        public class Archived
        {
            public const string Code = "archived";

            public const string Description = "Archived";
        }

        public class Deleted
        {
            public const string Code = "deleted";

            public const string Description = "Deleted";
        }
    }
}