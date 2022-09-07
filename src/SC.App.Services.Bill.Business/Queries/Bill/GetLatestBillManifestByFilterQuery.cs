using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Bill
{
    public class GetLatestBillManifestByFilter
    {
        [JsonProperty("live_id")]
        public Guid LiveId { get; set; }

        public GetLatestBillManifestByFilter(Guid liveId)
        {
            LiveId = liveId;
        }
    }

    public class GetLatestBillManifestByFilterQuery : BaseQuery, IRequest<Response<List<GetBillManifestResponse>>>
    {
        public GetLatestBillManifestByFilter Payload { get; private set; }

        public GetLatestBillManifestByFilterQuery(HttpRequest request, GetLatestBillManifestByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}