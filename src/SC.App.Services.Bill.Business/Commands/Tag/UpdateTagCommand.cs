using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Customer.Client;

namespace SC.App.Services.Bill.Business.Commands.Tag
{
    public class UpdateTag
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }

        public UpdateTag(Guid id, string name, string description, Guid userId)
        {
            Id = id;
            Name = name;
            Description = description;
            UserId = userId;
        }
    }

    public class UpdateTagCommand : BaseCommand, IRequest<Response<UpdateTagResponse>>
    {
        public UpdateTag Payload { get; private set; }

        public UpdateTagCommand(HttpRequest request, UpdateTag payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}