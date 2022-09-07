using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.BillNotification
{
    public class GetBillNotificationSummaryByFilter
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("period")]
        public EnumPeriod Period { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; private set; }

        public GetBillNotificationSummaryByFilter(Guid channelId, EnumPeriod period, DateTime date)
        {
            ChannelId = channelId;
            Period = period;
            Date = date;
        }
    }

    public class GetBillNotificationSummaryByFilterQuery : BaseQuery, IRequest<Response<GetBillNotificationSummaryResponse>>
    {
        public GetBillNotificationSummaryByFilter Payload { get; private set; }

        public GetBillNotificationSummaryByFilterQuery(HttpRequest request, GetBillNotificationSummaryByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}