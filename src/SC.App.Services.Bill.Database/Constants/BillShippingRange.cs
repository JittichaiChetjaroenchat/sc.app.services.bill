namespace SC.App.Services.Bill.Database.Constants
{
    public class BillShippingRange
    {
        public const string TableName = "bill_shipping_ranges";

        public static class Column
        {
            public const string Id = "id";

            public const string BillShippingRangeRuleId = "bill_shipping_range_rule_id";

            public const string Begin = "begin";

            public const string End = "end";

            public const string Cost = "cost";
        }
    }
}