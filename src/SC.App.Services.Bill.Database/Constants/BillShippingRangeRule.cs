namespace SC.App.Services.Bill.Database.Constants
{
    public class BillShippingRangeRule
    {
        public const string TableName = "bill_shipping_range_rules";

        public static class Column
        {
            public const string Id = "id";

            public const string BillShippingId = "bill_shipping_id";

            public const string Enabled = "enabled";
        }
    }
}