namespace SC.App.Services.Bill.Database.Constants
{
    public class BillConfiguration
    {
        public const string TableName = "bill_configurations";

        public static class Column
        {
            public const string Id = "id";

            public const string ChannelId = "channel_id";

            public const string CurrentNo = "current_no";

            public const string CreatedBy = "created_by";

            public const string CreatedOn = "created_on";
        }
    }
}