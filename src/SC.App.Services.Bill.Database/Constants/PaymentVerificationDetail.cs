namespace SC.App.Services.Bill.Database.Constants
{
    public class PaymentVerificationDetail
    {
        public const string TableName = "payment_verification_details";

        public static class Column
        {
            public const string Id = "id";

            public const string PaymentVerificationId = "payment_verification_id";

            public const string SourceBankCode = "source_bank_code";

            public const string SourceBankAccountType = "source_bank_account_type";

            public const string SourceBankAccountNumber = "source_bank_account_number";

            public const string SourceBankAccountName = "source_bank_account_name";

            public const string SourceBankAccountDisplayName = "source_bank_account_display_name";

            public const string DestinationBankCode = "destination_bank_code";

            public const string DestinationBankAccountType = "destination_bank_account_type";

            public const string DestinationBankAccountNumber = "destination_bank_account_number";

            public const string DestinationBankAccountName = "destination_bank_account_name";

            public const string DestinationBankAccountDisplayName = "destination_bank_account_display_name";

            public const string Amount = "amount";

            public const string TransactionRefNo = "transaction_ref_no";

            public const string TransactionDate = "transaction_date";
        }
    }
}