using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.Tag
{
    public class GetTagResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}