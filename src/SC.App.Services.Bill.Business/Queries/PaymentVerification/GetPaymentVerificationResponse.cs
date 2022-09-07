using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.PaymentVerification
{
    public class GetPaymentVerificationResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("is_proceed")]
        public bool IsProceed { get; set; }

        [JsonProperty("can_verify")]
        public bool CanVerify { get; set; }

        [JsonProperty("is_unique")]
        public bool IsUnique { get; set; }

        [JsonProperty("duplicate_to")]
        public string DuplicateTo { get; set; }

        [JsonProperty("is_correct_bank_account_number")]
        public bool IsCorrectBankAccountNumber { get; set; }

        [JsonProperty("is_correct_bank_account_name")]
        public bool IsCorrectBankAccountName { get; set; }

        [JsonProperty("is_correct_amount")]
        public bool IsCorrectAmount { get; set; }

        [JsonProperty("unbalance_amount")]
        public decimal UnBalanceAmount { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("detail")]
        public GetPaymentVerificationDetail Detail { get; set; }

        [JsonProperty("status")]
        public GetPaymentVerificationStatus Status { get; set; }
    }

    public class GetPaymentVerificationDetail
    {
        [JsonProperty("source")]
        public GetPaymentVerificationBankAccount Source { get; set; }

        [JsonProperty("destination")]
        public GetPaymentVerificationBankAccount Destination { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("transaction_ref_no")]
        public string TransactionRefNo { get; set; }

        [JsonProperty("transaction_date")]
        public GetPaymentVerificationDate TransactionDate { get; set; }
    }

    public class GetPaymentVerificationBankAccount
    {
        [JsonProperty("bank")]
        public GetPaymentVerificationBank Bank { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class GetPaymentVerificationBank
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class GetPaymentVerificationStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class GetPaymentVerificationDate
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("is_present_date")]
        public bool IsPresentDate { get; set; }

        [JsonProperty("is_present_month")]
        public bool IsPresentMonth { get; set; }

        [JsonProperty("is_present_year")]
        public bool IsPresentYear { get; set; }
    }
}