using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Queue.Models.Order
{
    public class NotifyOrderConfirm
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }

        public NotifyOrderConfirm(Guid id, string recipientId)
        {
            Id = id;
            RecipientId = recipientId;
        }
    }
}