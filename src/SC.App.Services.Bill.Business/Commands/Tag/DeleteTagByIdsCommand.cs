using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Tag
{
    public class DeleteTagByIds
    {
        [JsonProperty("ids")]
        public Guid[] Ids { get; private set; }

        public DeleteTagByIds(Guid[] ids)
        {
            Ids = ids;
        }
    }

    public class DeleteTagByIdsCommand : BaseCommand, IRequest<Response<DeleteTagByIdsResponse>>
    {
        public DeleteTagByIds Payload { get; private set; }

        public DeleteTagByIdsCommand(HttpRequest request, DeleteTagByIds payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}