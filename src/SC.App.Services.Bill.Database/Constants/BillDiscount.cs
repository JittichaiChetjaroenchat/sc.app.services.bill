namespace SC.App.Services.Bill.Database.Constants
{
    public class BillDiscount
    {
        public const string TableName = "bill_discounts";

        public static class Column
        {
            public const string Id = "id";

            public const string BillId = "bill_id";

            public const string Amount = "amount";

            public const string Percentage = "percentage";

            public const string CreatedBy = "created_by";

            public const string CreatedOn = "created_on";

            public const string UpdatedBy = "updated_by";

            public const string UpdatedOn = "updated_on";
        }
    }
}