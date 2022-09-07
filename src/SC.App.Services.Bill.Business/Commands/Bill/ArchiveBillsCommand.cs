using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class ArchiveBills
    {
        [JsonProperty("ids")]
        public Guid[] Ids { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class ArchiveBillsCommand : BaseCommand, IRequest<Response<ArchiveBillsResponse>>
    {
        public ArchiveBills Payload { get; set; }

        public ArchiveBillsCommand(HttpRequest request, ArchiveBills payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}