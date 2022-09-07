using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Commands.Payment
{
    public class OpenPaymentResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public OpenPaymentResponse(Guid id)
        {
            Id = id;
        }
    }
}