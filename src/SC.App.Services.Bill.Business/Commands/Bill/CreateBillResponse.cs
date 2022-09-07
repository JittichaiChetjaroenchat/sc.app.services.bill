using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class CreateBillResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public CreateBillResponse(Guid id)
        {
            Id = id;
        }
    }
}
