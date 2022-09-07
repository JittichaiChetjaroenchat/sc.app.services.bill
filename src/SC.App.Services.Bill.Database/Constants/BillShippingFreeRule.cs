namespace SC.App.Services.Bill.Database.Constants
{
    public class BillShippingFreeRule
    {
        public const string TableName = "bill_shipping_free_rules";

        public static class Column
        {
            public const string Id = "id";

            public const string BillShippingId = "bill_shipping_id";

            public const string Amount = "amount";

            public const string Price = "price";

            public const string Enabled = "enabled";
        }
    }
}