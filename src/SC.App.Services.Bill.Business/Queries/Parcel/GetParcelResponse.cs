using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.Parcel
{
    public class GetParcelResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("status")]
        public GetParcelStatus Status { get; set; }

        [JsonProperty("created_on")]
        public GetParcelDate CreatedOn { get; set; }

        [JsonProperty("is_printed")]
        public bool IsPrinted { get; set; }

        [JsonProperty("is_packed")]
        public bool IsPacked { get; set; }
    }

    public class GetParcelDate
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

    public class GetParcelStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}