namespace SC.App.Services.Bill.Database.Constants
{
    public class Payment
    {
        public const string TableName = "payments";

        public static class Column
        {
            public const string Id = "id";

            public const string BillId = "bill_id";

            public const string PaymentNo = "payment_no";

            public const string Amount = "amount";

            public const string EvidenceId = "evidence_id";

            public const string PayOn = "pay_on";

            public const string Remark = "remark";

            public const string PaymentStatusId = "payment_status_id";

            public const string CreatedOn = "created_on";
        }
    }
}