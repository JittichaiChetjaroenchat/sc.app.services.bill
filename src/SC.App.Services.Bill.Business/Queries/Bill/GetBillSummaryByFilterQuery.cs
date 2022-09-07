using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Bill
{
    public class GetBillSummaryByFilter
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("period")]
        public EnumPeriod Period { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; private set; }

        public GetBillSummaryByFilter(Guid channelId, EnumPeriod period, DateTime date)
        {
            ChannelId = channelId;
            Period = period;
            Date = date;
        }
    }

    public class GetBillSummaryByFilterQuery : BaseQuery, IRequest<Response<GetBillSummaryResponse>>
    {
        public GetBillSummaryByFilter Payload { get; private set; }

        public GetBillSummaryByFilterQuery(HttpRequest request, GetBillSummaryByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}