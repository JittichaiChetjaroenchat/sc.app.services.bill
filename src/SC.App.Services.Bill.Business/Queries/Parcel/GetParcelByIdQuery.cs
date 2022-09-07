using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Parcel
{
    public class GetParcelById
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public GetParcelById(Guid id)
        {
            Id = id;
        }
    }

    public class GetParcelByIdQuery : BaseQuery, IRequest<Response<GetParcelResponse>>
    {
        public GetParcelById Payload { get; private set; }

        public GetParcelByIdQuery(HttpRequest request, GetParcelById payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}