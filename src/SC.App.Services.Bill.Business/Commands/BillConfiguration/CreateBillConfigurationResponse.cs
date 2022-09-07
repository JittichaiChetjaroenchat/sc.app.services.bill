using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Commands.BillConfiguration
{
    public class CreateBillConfigurationResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public CreateBillConfigurationResponse(Guid id)
        {
            Id = id;
        }
    }
}