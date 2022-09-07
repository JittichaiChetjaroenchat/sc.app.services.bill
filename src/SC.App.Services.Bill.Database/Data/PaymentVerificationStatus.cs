namespace SC.App.Services.Bill.Database.Data
{
    public class PaymentVerificationStatus
    {
        public class NotVerify
        {
            public const string Code = "not_verify";

            public const string Description = "Not verify";
        }

        public class Unverifiable
        {
            public const string Code = "unverifiable";

            public const string Description = "Unverifiable";
        }

        public class Duplicate
        {
            public const string Code = "duplicate";

            public const string Description = "Duplicate";
        }

        public class IncorrectBankAccountNumber
        {
            public const string Code = "incorrect_bank_account_number";

            public const string Description = "Incorrect bank account's number";
        }

        public class IncorrectBankAccountName
        {
            public const string Code = "incorrect_bank_account_name";

            public const string Description = "Incorrect bank account's name";
        }

        public class IncorrectAmount
        {
            public const string Code = "incorrect_amount";

            public const string Description = "Incorrect amount";
        }

        public class Verified
        {
            public const string Code = "verified";

            public const string Description = "Verified";
        }
    }
}