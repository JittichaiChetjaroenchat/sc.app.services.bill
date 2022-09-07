using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Bill
{
    public class GetBillById
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public GetBillById(Guid id)
        {
            Id = id;
        }
    }

    public class GetBillByIdQuery : BaseQuery, IRequest<Response<GetBillResponse>>
    {
        public GetBillById Payload { get; private set; }

        public GetBillByIdQuery(HttpRequest request, GetBillById payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}