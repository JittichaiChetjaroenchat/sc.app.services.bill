using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.Bill
{
    public class GetBillSummaryResponse
    {
        [JsonProperty("all")]
        public int All { get; set; }

        [JsonProperty("pending")]
        public int Pending { get; set; }

        [JsonProperty("notified")]
        public int Notified { get; set; }

        [JsonProperty("rejected")]
        public int Rejected { get; set; }

        [JsonProperty("confirmed")]
        public int Confirmed { get; set; }

        [JsonProperty("deposited")]
        public int Deposited { get; set; }

        [JsonProperty("cod")]
        public int Cod { get; set; }

        [JsonProperty("cancelled")]
        public int Cancelled { get; set; }

        [JsonProperty("done")]
        public int Done { get; set; }

        [JsonProperty("printing")]
        public int Printing { get; set; }

        [JsonProperty("printed")]
        public int Printed { get; set; }

        [JsonProperty("archived")]
        public int Archived { get; set; }
    }
}