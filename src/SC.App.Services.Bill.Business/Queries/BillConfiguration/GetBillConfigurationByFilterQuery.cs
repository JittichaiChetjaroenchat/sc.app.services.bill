using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.BillConfiguration
{
    public class GetBillConfigurationByFilter
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        public GetBillConfigurationByFilter(Guid channelId)
        {
            ChannelId = channelId;
        }
    }

    public class GetBillConfigurationByFilterQuery : BaseQuery, IRequest<Response<GetBillConfigurationResponse>>
    {
        public GetBillConfigurationByFilter Payload { get; private set; }

        public GetBillConfigurationByFilterQuery(HttpRequest request, GetBillConfigurationByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}