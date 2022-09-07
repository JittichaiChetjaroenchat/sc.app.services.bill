using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Queue.Models.Bill
{
    public class NotifyBillCancel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public NotifyBillCancel(Guid id)
        {
            Id = id;
        }
    }
}