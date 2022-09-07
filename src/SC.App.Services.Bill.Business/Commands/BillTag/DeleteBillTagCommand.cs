using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.BillTag
{
    public class DeleteBillTag
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public DeleteBillTag(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteBillTagCommand : BaseCommand, IRequest<Response<DeleteBillTagResponse>>
    {
        public DeleteBillTag Payload { get; private set; }

        public DeleteBillTagCommand(HttpRequest request, DeleteBillTag payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}