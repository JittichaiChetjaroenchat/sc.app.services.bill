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
    public class UpdateOfflineBill
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("live_id")]
        public Guid? LiveId { get; set; }

        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        [JsonProperty("recipient")]
        public UpdateOfflineBillRecipient Recipient { get; set; }

        [JsonProperty("orders")]
        public List<UpdateOfflineBillOrder> Orders { get; set; }

        [JsonProperty("payment")]
        public UpdateOfflineBillPayment Payment { get; set; }

        [JsonProperty("shipping")]
        public UpdateOfflineBillShipping Shipping { get; set; }

        [JsonProperty("discount")]
        public UpdateOfflineBillDiscount Discount { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class UpdateOfflineBillRecipient
    {
        [JsonProperty("id")]
        public Guid? Id { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contact")]
        public UpdateOfflineBillRecipientContact Contact { get; set; }
    }

    public class UpdateOfflineBillRecipientContact
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

    public class UpdateOfflineBillOrder
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

    public class UpdateOfflineBillPayment
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

    public class UpdateOfflineBillShipping
    {
        [JsonProperty("cost_type")]
        public EnumShippingCostType CostType { get; set; }
    }

    public class UpdateOfflineBillDiscount
    {
        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("percentage")]
        public decimal? Percentage { get; set; }
    }

    public class UpdateOfflineBillCommand : BaseCommand, IRequest<Response<UpdateBillResponse>>
    {
        public UpdateOfflineBill Payload { get; set; }

        public UpdateOfflineBillCommand(HttpRequest request, UpdateOfflineBill payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}