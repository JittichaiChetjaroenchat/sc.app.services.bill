namespace SC.App.Services.Bill.Database.Constants
{
    public class BillShippingTotalRule
    {
        public const string TableName = "bill_shipping_total_rules";

        public static class Column
        {
            public const string Id = "id";

            public const string BillShippingId = "bill_shipping_id";

            public const string Cost = "cost";

            public const string Enabled = "enabled";
        }
    }
}