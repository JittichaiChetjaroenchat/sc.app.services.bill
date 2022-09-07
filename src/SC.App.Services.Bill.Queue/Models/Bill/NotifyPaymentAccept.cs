using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Queue.Models.Bill
{
    public class NotifyPaymentAccept
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public NotifyPaymentAccept(Guid id)
        {
            Id = id;
        }
    }
}