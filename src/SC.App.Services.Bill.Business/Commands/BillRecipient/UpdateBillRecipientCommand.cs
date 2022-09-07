using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.BillRecipient
{
    public class UpdateBillRecipient
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias_name")]
        public string AliasName { get; set; }

        [JsonProperty("contact")]
        public UpdateBillRecipientContact Contact { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class UpdateBillRecipientContact
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("area_id")]
        public Guid AreaId { get; set; }

        [JsonProperty("primary_phone")]
        public string PrimaryPhone { get; set; }

        [JsonProperty("secondary_phone")]
        public string SecondaryPhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class UpdateBillRecipientCommand : BaseCommand, IRequest<Response<UpdateBillRecipientResponse>>
    {
        public UpdateBillRecipient Payload { get; set; }

        public UpdateBillRecipientCommand(HttpRequest request, UpdateBillRecipient payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}