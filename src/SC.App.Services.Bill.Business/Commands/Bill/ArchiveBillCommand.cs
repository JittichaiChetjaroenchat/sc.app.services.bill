using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class ArchiveBill
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class ArchiveBillCommand : BaseCommand, IRequest<Response<ArchiveBillResponse>>
    {
        public ArchiveBill Payload { get; set; }

        public ArchiveBillCommand(HttpRequest request, ArchiveBill payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}