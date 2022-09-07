using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.BillNotification
{
    public class SearchBillNotificationByFilter
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; private set; }

        [JsonProperty("status")]
        public EnumSearchBillNotificationStatus Status { get; private set; }

        [JsonProperty("period")]
        public EnumPeriod Period { get; private set; }

        [JsonProperty("date")]
        public DateTime Date { get; private set; }

        [JsonProperty("keyword")]
        public string Keyword { get; private set; }

        [JsonProperty("sort_by")]
        public string SortBy { get; private set; }

        [JsonProperty("sort_desc")]
        public bool SortDesc { get; private set; }

        [JsonProperty("page")]
        public int Page { get; private set; }

        [JsonProperty("page_size")]
        public int PageSize { get; private set; }

        public SearchBillNotificationByFilter(Guid channelId, EnumSearchBillNotificationStatus status, EnumPeriod period, DateTime date, string keyword, string sortBy, bool sortDesc, int page, int pageSize)
        {
            ChannelId = channelId;
            Status = status;
            Period = period;
            Date = date;
            Keyword = keyword;
            SortBy = sortBy;
            SortDesc = sortDesc;
            Page = page;
            PageSize = pageSize;
        }
    }

    public class SearchBillNotificationByFilterQuery : BaseQuery, IRequest<Response<SearchBillNotificationResponse>>
    {
        public SearchBillNotificationByFilter Payload { get; private set; }

        public SearchBillNotificationByFilterQuery(HttpRequest request, SearchBillNotificationByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}