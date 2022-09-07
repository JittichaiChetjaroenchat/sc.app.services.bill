namespace SC.App.Services.Bill.Database.Data
{
    public class ParcelStatus
    {
        public class Active
        {
            public const string Code = "active";

            public const string Description = "Active";
        }

        public class Cancelled
        {
            public const string Code = "cancelled";

            public const string Description = "Cancelled";
        }
    }
}