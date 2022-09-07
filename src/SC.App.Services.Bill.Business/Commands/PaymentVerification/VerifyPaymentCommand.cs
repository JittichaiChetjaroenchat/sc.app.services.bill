using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.PaymentVerification
{
    public class VerifyPayment
    {
        [JsonProperty("payment_verification_id")]
        public Guid PaymentVerificationId { get; set; }

        public VerifyPayment(Guid paymentVerificationId)
        {
            PaymentVerificationId = paymentVerificationId;
        }
    }

    public class VerifyPaymentCommand : BaseCommand, IRequest<Response<VerifyPaymentResponse>>
    {
        public VerifyPayment Payload { get; private set; }

        public VerifyPaymentCommand(HttpRequest request, VerifyPayment payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}