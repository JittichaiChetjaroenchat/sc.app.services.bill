using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Commands.BillTag
{
    public class CreateBillTagResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        public CreateBillTagResponse(Guid id)
        {
            Id = id;
        }
    }
}