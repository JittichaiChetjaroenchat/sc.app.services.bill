using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Tag
{
    public class CreateTag
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CreateTagCommand : BaseCommand, IRequest<Response<CreateTagResponse>>
    {
        public CreateTag Payload { get; private set; }

        public CreateTagCommand(HttpRequest request, CreateTag payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}