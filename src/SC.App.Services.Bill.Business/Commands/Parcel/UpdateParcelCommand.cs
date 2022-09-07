using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Parcel
{
    public class UpdateParcel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("orders")]
        public List<UpdateParcelOrder> Orders { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class UpdateParcelOrder
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class UpdateParcelCommand : BaseCommand, IRequest<Response<UpdateParcelResponse>>
    {
        public UpdateParcel Payload { get; set; }

        public UpdateParcelCommand(HttpRequest request, UpdateParcel payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}