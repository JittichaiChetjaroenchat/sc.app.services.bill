using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Parcel
{
    public class CreateParcels
    {
        [JsonProperty("bill_ids")]
        public Guid[] BillIds { get; set; }

        [JsonProperty("option")]
        public CreateParcelsOption Option { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CreateParcelsOption
    {
        [JsonProperty("shipping_type_id")]
        public Guid ShippingTypeId { get; set; }

        [JsonProperty("velocity_type_id")]
        public Guid VelocityTypeId { get; set; }

        [JsonProperty("insurance_type_id")]
        public Guid InsuranceTypeId { get; set; }
    }

    public class CreateParcelsCommand : BaseCommand, IRequest<Response<CreateParcelsResponse>>
    {
        public CreateParcels Payload { get; set; }

        public CreateParcelsCommand(HttpRequest request, CreateParcels payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}