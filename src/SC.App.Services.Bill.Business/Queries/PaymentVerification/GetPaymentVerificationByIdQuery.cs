using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.PaymentVerification
{
    public class GetPaymentVerificationById
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        public GetPaymentVerificationById(Guid id)
        {
            Id = id;
        }
    }

    public class GetPaymentVerificationByIdQuery : BaseQuery, IRequest<Response<GetPaymentVerificationResponse>>
    {
        public GetPaymentVerificationById Payload { get; private set; }

        public GetPaymentVerificationByIdQuery(HttpRequest request, GetPaymentVerificationById payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}