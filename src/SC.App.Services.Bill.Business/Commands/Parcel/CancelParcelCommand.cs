using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Parcel
{
    public class CancelParcel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CancelParcelCommand : BaseCommand, IRequest<Response<CancelParcelResponse>>
    {
        public CancelParcel Payload { get; set; }

        public CancelParcelCommand(HttpRequest request, CancelParcel payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}