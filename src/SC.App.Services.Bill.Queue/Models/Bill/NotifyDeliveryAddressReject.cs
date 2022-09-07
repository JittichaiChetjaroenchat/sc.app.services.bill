using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Queue.Models.Bill
{
    public class NotifyDeliveryAddressReject
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public NotifyDeliveryAddressReject(Guid id)
        {
            Id = id;
        }
    }
}