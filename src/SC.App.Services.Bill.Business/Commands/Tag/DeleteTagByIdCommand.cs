using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Tag
{
    public class DeleteTagById
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        public DeleteTagById(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteTagByIdCommand : BaseCommand, IRequest<Response<DeleteTagByIdResponse>>
    {
        public DeleteTagById Payload { get; private set; }

        public DeleteTagByIdCommand(HttpRequest request, DeleteTagById payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}