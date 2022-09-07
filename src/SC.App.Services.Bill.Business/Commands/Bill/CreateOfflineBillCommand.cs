using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class CreateOfflineBill
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("recipient")]
        public CreateOfflineBillRecipient Recipient { get; set; }

        [JsonProperty("orders")]
        public List<CreateOfflineBillOrder> Orders { get; set; }

        [JsonProperty("payment")]
        public CreateOfflineBillPayment Payment { get; set; }

        [JsonProperty("shipping")]
        public CreateOfflineBillShipping Shipping { get; set; }

        [JsonProperty("discount")]
        public CreateOfflineBillDiscount Discount { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CreateOfflineBillRecipient
    {
        [JsonProperty("id")]
        public Guid? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias_name")]
        public string AliasName { get; set; }

        [JsonProperty("contact")]
        public CreateOfflineBillRecipientContact Contact { get; set; }
    }

    public class CreateOfflineBillRecipientContact
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("area_id")]
        public Guid? AreaId { get; set; }

        [JsonProperty("primary_phone")]
        public string PrimaryPhone { get; set; }

        [JsonProperty("secondary_phone")]
        public string SecondaryPhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class CreateOfflineBillOrder
    {
        [JsonProperty("product_id")]
        public Guid ProductId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }
    }

    public class CreateOfflineBillPayment
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("has_cod_addon")]
        public bool HasCodAddOn { get; set; }

        [JsonProperty("cod_addon_amount")]
        public decimal? CodAddOnAmount { get; set; }

        [JsonProperty("cod_addon_percentage")]
        public decimal? CodAddOnPercentage { get; set; }

        [JsonProperty("has_vat")]
        public bool HasVat { get; set; }

        [JsonProperty("included_vat")]
        public bool IncludedVat { get; set; }
    }

    public class CreateOfflineBillDiscount
    {
        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("percentage")]
        public decimal? Percentage { get; set; }
    }

    public class CreateOfflineBillShipping
    {
        [JsonProperty("cost_type")]
        public EnumShippingCostType CostType { get; set; }
    }

    public class CreateOfflineBillCommand : BaseCommand, IRequest<Response<CreateBillResponse>>
    {
        public CreateOfflineBill Payload { get; set; }

        public CreateOfflineBillCommand(HttpRequest request, CreateOfflineBill payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}