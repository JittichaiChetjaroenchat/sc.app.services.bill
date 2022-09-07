using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Tag
{
    public class GetTagByFilter
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; private set; }

        public GetTagByFilter(Guid channelId)
        {
            ChannelId = channelId;
        }
    }

    public class GetTagByFilterQuery : BaseQuery, IRequest<Response<List<GetTagResponse>>>
    {
        public GetTagByFilter Payload { get; private set; }

        public GetTagByFilterQuery(HttpRequest request, GetTagByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}