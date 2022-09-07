using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.Bill
{
    public class GetBillManifestResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("is_deposit")]
        public bool IsDeposit { get; set; }

        [JsonProperty("customer")]
        public GetBillManifestCustomer Customer { get; set; }

        [JsonProperty("goods")]
        public GetBillManifestGoods Goods { get; set; }

        [JsonProperty("shipping")]
        public GetBillManifestShipping Shipping { get; set; }
        
        [JsonProperty("latest_payment")]
        public GetBillManifestLatestPayment LatestPayment { get; set; }

        [JsonProperty("status")]
        public GetBillManifestStatus Status { get; set; }
    }

    public class GetBillManifestCustomer
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class GetBillManifestLatestPayment
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class GetBillManifestGoods
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("cost")]
        public decimal Cost { get; set; }
    }

    public class GetBillManifestShipping
    {
        [JsonProperty("cost")]
        public decimal Cost { get; set; }
    }

    public class GetBillManifestStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}