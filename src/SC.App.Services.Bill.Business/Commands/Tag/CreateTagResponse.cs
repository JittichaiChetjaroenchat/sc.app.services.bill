using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Commands.Tag
{
    public class CreateTagResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        public CreateTagResponse(Guid id)
        {
            Id = id;
        }
    }
}