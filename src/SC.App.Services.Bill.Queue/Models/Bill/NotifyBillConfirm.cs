using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Queue.Models.Bill
{
    public class NotifyBillConfirm
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public NotifyBillConfirm(Guid id)
        {
            Id = id;
        }
    }
}