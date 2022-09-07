namespace SC.App.Services.Bill.Database.Constants
{
    public class Bill
    {
        public const string TableName = "bills";

        public static class Column
        {
            public const string Id = "id";

            public const string ChannelId = "channel_id";

            public const string BillNo = "bill_no";

            public const string RunningNo = "running_no";

            public const string BillChannelId = "bill_channel_id";

            public const string BillStatusId = "bill_status_id";

            public const string IsDeposit = "is_deposit";

            public const string IsNewCustomer = "is_new_customer";

            public const string Remark = "remark";

            public const string Key = "key";

            public const string CreatedBy = "created_by";

            public const string CreatedOn = "created_on";

            public const string UpdatedBy = "updated_by";

            public const string UpdatedOn = "updated_on";
        }
    }
}