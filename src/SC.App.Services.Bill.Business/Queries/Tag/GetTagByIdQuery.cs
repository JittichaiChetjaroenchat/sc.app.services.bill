using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Tag
{
    public class GetTagById
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        public GetTagById(Guid id)
        {
            Id = id;
        }
    }

    public class GetTagByIdQuery : BaseQuery, IRequest<Response<GetTagResponse>>
    {
        public GetTagById Payload { get; private set; }

        public GetTagByIdQuery(HttpRequest request, GetTagById payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}