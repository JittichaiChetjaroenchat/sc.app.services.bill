using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Queue.Models.Bill
{
    public class NotifyDeliveryAddressAccept
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public NotifyDeliveryAddressAccept(Guid id)
        {
            Id = id;
        }
    }
}