namespace SC.App.Services.Bill.Database.Constants
{
    public class BillTag
    {
        public const string TableName = "bill_tags";

        public static class Column
        {
            public const string Id = "id";

            public const string BillId = "bill_id";

            public const string TagId = "tag_id";

            public const string CreatedBy = "created_by";

            public const string CreatedOn = "created_on";

            public const string UpdatedBy = "updated_by";

            public const string UpdatedOn = "updated_on";
        }
    }
}