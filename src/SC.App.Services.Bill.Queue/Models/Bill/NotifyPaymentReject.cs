using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Queue.Models.Bill
{
    public class NotifyPaymentReject
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public NotifyPaymentReject(Guid id)
        {
            Id = id;
        }
    }
}