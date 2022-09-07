using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Bill
{
    public class SearchBillByFilter
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; private set; }

        [JsonProperty("status")]
        public EnumSearchBillStatus Status { get; private set; }

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

        public SearchBillByFilter(Guid channelId, EnumSearchBillStatus status, EnumPeriod period, DateTime date, string keyword, string sortBy, bool sortDesc, int page, int pageSize)
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

    public class SearchBillByFilterQuery : BaseQuery, IRequest<Response<SearchBillResponse>>
    {
        public SearchBillByFilter Payload { get; private set; }

        public SearchBillByFilterQuery(HttpRequest request, SearchBillByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}