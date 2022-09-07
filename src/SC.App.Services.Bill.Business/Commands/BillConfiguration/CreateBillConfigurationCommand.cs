using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.BillConfiguration
{
    public class CreateBillConfiguration
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CreateBillConfigurationCommand : BaseCommand, IRequest<Response<CreateBillConfigurationResponse>>
    {
        public CreateBillConfiguration Payload { get; private set; }

        public CreateBillConfigurationCommand(HttpRequest request, CreateBillConfiguration payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}