namespace SC.App.Services.Bill.Database.Constants
{
    public class BillRecipient
    {
        public const string TableName = "bill_recipients";

        public static class Column
        {
            public const string Id = "id";

            public const string BillId = "bill_id";

            public const string CustomerId = "customer_id";

            public const string Name = "name";

            public const string AliasName = "alias_name";

            public const string CreatedBy = "created_by";

            public const string CreatedOn = "created_on";

            public const string UpdatedBy = "updated_by";

            public const string UpdatedOn = "updated_on";
        }
    }
}