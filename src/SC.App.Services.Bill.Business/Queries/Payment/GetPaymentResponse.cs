using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.Payment
{
    public class GetPaymentResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("payment_no")]
        public string PaymentNo { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("pay_on")]
        public GetPaymentDate PayOn { get; set; }

        [JsonProperty("evidence")]
        public GetPaymentEvidence Evidence { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("status")]
        public GetPaymentStatus Status { get; set; }

        [JsonProperty("created_on")]
        public GetPaymentDate CreatedOn { get; set; }
    }

    public class GetPaymentDate
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

    public class GetPaymentEvidence
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("content")]
        public byte[] Content { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("content_length")]
        public int ContentLength { get; set; }
    }

    public class GetPaymentStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}