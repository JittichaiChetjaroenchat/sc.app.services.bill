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
    public class CreateOnlineBill
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("live_id")]
        public Guid? LiveId { get; set; }

        [JsonProperty("post_id")]
        public Guid? PostId { get; set; }

        [JsonProperty("bill_channel")]
        public EnumBillChannel BillChannel { get; set; }

        [JsonProperty("recipient")]
        public CreateOnlineBillRecipient Recipient { get; set; }

        [JsonProperty("orders")]
        public List<CreateOnlineBillOrder> Orders { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CreateOnlineBillRecipient
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias_name")]
        public string AliasName { get; set; }
    }

    public class CreateOnlineBillOrder
    {
        [JsonProperty("booking_id")]
        public Guid? BookingId { get; set; }

        [JsonProperty("product_id")]
        public Guid ProductId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }
    }

    public class CreateOnlineBillCommand : BaseCommand, IRequest<Response<CreateBillResponse>>
    {
        public CreateOnlineBill Payload { get; set; }

        public CreateOnlineBillCommand(HttpRequest request, CreateOnlineBill payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}