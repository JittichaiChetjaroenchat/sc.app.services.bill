using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.BillConfiguration
{
    public class GetBillConfigurationResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("current_no")]
        public int CurrentNo { get; set; }
    }
}