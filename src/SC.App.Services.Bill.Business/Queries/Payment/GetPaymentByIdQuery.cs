using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Payment
{
    public class GetPaymentById
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public GetPaymentById(Guid id)
        {
            Id = id;
        }
    }

    public class GetPaymentByIdQuery : BaseQuery, IRequest<Response<GetPaymentResponse>>
    {
        public GetPaymentById Payload { get; private set; }

        public GetPaymentByIdQuery(HttpRequest request, GetPaymentById payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}