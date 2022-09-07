using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Commands.Parcel
{
    public class CreateParcelResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public CreateParcelResponse(Guid id)
        {
            Id = id;
        }
    }
}