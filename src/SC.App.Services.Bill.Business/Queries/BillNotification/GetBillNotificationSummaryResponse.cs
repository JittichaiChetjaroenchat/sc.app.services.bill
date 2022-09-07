using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.BillNotification
{
    public class GetBillNotificationSummaryResponse
    {
        [JsonProperty("all")]
        public int All { get; set; }

        [JsonProperty("sent_summary")]
        public int SentSummary { get; set; }

        [JsonProperty("unsent_summary")]
        public int UnsentSummary { get; set; }
    }
}