using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Parcel
{
    public class GetParcelByFilter
    {
        [JsonProperty("bill_id")]
        public Guid? BillId { get; set; }

        [JsonProperty("id")]
        public Guid[] Ids { get; set; }

        public GetParcelByFilter(Guid? bill_id, Guid[] ids)
        {
            BillId = bill_id;
            Ids = ids;
        }
    }

    public class GetParcelByFilterQuery : BaseQuery, IRequest<Response<List<GetParcelResponse>>>
    {
        public GetParcelByFilter Payload { get; private set; }

        public GetParcelByFilterQuery(HttpRequest request, GetParcelByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}