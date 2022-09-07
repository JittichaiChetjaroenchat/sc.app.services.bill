namespace SC.App.Services.Bill.Database.Constants
{
    public class PaymentVerification
    {
        public const string TableName = "payment_verifications";

        public static class Column
        {
            public const string Id = "id";

            public const string PaymentId = "payment_id";

            public const string IsProceed = "is_proceed";

            public const string CanVerify = "can_verifiy";

            public const string IsUnique = "is_unique";

            public const string DuplicateTo = "duplicate_to";

            public const string IsCorrectBankAccountNumber = "is_correct_bank_account_number";

            public const string IsCorrectBankAccountName = "is_correct_bank_account_name";

            public const string IsCorrectAmount = "is_correct_amount";

            public const string UnBalanceAmount = "unbalance_amount";

            public const string Remark = "remark";

            public const string PaymentVerificationStatusId = "payment_verification_status_id";

            public const string CreatedOn = "created_on";
        }
    }
}