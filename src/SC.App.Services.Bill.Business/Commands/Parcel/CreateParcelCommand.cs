using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Parcel
{
    public class CreateParcel
    {
        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        [JsonProperty("orders")]
        public List<CreateParcelOrder> Orders { get; set; }

        [JsonProperty("option")]
        public CreateParcelOption Option { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CreateParcelOrder
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class CreateParcelOption
    {
        [JsonProperty("shipping_type_id")]
        public Guid ShippingTypeId { get; set; }

        [JsonProperty("velocity_type_id")]
        public Guid VelocityTypeId { get; set; }

        [JsonProperty("payment_type_id")]
        public Guid PaymentTypeId { get; set; }

        [JsonProperty("cod_amount")]
        public decimal CodAmount { get; set; }

        [JsonProperty("insurance_type_id")]
        public Guid InsuranceTypeId { get; set; }

        [JsonProperty("insurance_amount")]
        public decimal InsuranceAmount { get; set; }
    }

    public class CreateParcelCommand : BaseCommand, IRequest<Response<CreateParcelResponse>>
    {
        public CreateParcel Payload { get; set; }

        public CreateParcelCommand(HttpRequest request, CreateParcel payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}