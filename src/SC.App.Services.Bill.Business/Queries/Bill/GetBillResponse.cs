using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SC.App.Services.Bill.Business.Queries.Payment;

namespace SC.App.Services.Bill.Business.Queries.Bill
{
    public class GetBillResponse
    {
        [JsonProperty("bill")]
        public GetBill Bill { get; set; }

        [JsonProperty("recipient")]
        public GetBillRecipient Recipient { get; set; }

        [JsonProperty("orders")]
        public List<GetBillOrder> Orders { get; set; }

        [JsonProperty("payment")]
        public GetPayment Payment { get; set; }

        [JsonProperty("shipping")]
        public GetBillShipping Shipping { get; set; }

        [JsonProperty("cost")]
        public GetBillCost Cost { get; set; }

        [JsonProperty("tags")]
        public List<GetBillTag> Tags { get; set; }
    }

    #region Bill

    public class GetBill
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("bill_no")]
        public string BillNo { get; set; }

        [JsonProperty("running_no")]
        public string RunningNo { get; set; }

        [JsonProperty("is_deposit")]
        public bool IsDeposit { get; set; }

        [JsonProperty("is_new_customer")]
        public bool IsNewCustomer { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("discount")]
        public GetBillDiscount Discount { get; set; }

        [JsonProperty("payment")]
        public GetBillPayment Payment { get; set; }

        [JsonProperty("status")]
        public GetBillStatus Status { get; set; }

        [JsonProperty("created_on")]
        public GetBillDate CreatedOn { get; set; }
    }

    public class GetBillPayment
    {
        [JsonProperty("type")]
        public GetBillPaymentType Type { get; set; }

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

        [JsonProperty("vat_percentage")]
        public decimal VatPercentage { get; set; }
    }

    public class GetBillPaymentType
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class GetBillDiscount
    {
        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("percentage")]
        public decimal? Percentage { get; set; }
    }

    public class GetBillStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class GetBillDate
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("is_present_date")]
        public bool IsPresentDate { get; set; }

        [JsonProperty("is_present_month")]
        public bool IsPresentMonth { get; set; }

        [JsonProperty("is_present_year")]
        public bool IsPresentYear { get; set; }
    }

    #endregion

    #region Recipient

    public class GetBillRecipient
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias_name")]
        public string AliasName { get; set; }

        [JsonProperty("contact")]
        public GetBillRecipientContact Contact { get; set; }

        [JsonProperty("customer")]
        public GetBillRecipientCustomer Customer { get; set; }
    }

    public class GetBillRecipientContact
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("area_id")]
        public Guid? AreaId { get; set; }

        [JsonProperty("sub_district")]
        public string SubDistrict { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("primary_phone")]
        public string PrimaryPhone { get; set; }

        [JsonProperty("secondary_phone")]
        public string SecondaryPhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class GetBillRecipientCustomer
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_new")]
        public bool IsNew { get; set; }

        [JsonProperty("is_blocked")]
        public bool IsBlocked { get; set; }

        [JsonProperty("facebook")]
        public GetBillRecipientCustomerFacebook Facebook { get; set; }

        [JsonProperty("tags")]
        public List<GetBillRecipientCustomerTag> Tags { get; set; }
    }

    public class GetBillRecipientCustomerFacebook
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class GetBillRecipientCustomerTag
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("tag_id")]
        public Guid TagId { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    #endregion

    #region Order

    public class GetBillOrder
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("parcel")]
        public GetBillOrderParcel Parcel { get; set; }

        [JsonProperty("product")]
        public GetBillOrderProduct Product { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("paid")]
        public bool Paid { get; set; }

        [JsonProperty("status")]
        public GetBillOrderStatus Status { get; set; }

        [JsonProperty("created_on")]
        public GetBillOrderDate CreatedOn { get; set; }
    }

    public class GetBillOrderParcel
    {
        [JsonProperty("id")]
        public Guid? Id { get; set; }
    }

    public class GetBillOrderProduct
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }
    }

    public class GetBillOrderStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class GetBillOrderDate
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("is_present_date")]
        public bool IsPresentDate { get; set; }

        [JsonProperty("is_present_month")]
        public bool IsPresentMonth { get; set; }

        [JsonProperty("is_present_year")]
        public bool IsPresentYear { get; set; }
    }

    #endregion

    #region Payment

    public class GetPayment
    {
        [JsonProperty("status")]
        public GetPaymentStatus Status { get; set; }
    }

    #endregion

    #region Shipping

    public class GetBillShipping
    {
        [JsonProperty("cost_type")]
        public string CostType { get; set; }

        [JsonProperty("parcels")]
        public List<GetBillParcel> Parcels { get; set; }
    }

    #endregion

    #region Parcel

    public class GetBillParcel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("status")]
        public GetBillParcelStatus Status { get; set; }

        [JsonProperty("created_on")]
        public GetBillParcelDate CreatedOn { get; set; }

        [JsonProperty("is_printed")]
        public bool IsPrinted { get; set; }

        [JsonProperty("is_packed")]
        public bool IsPacked { get; set; }
    }

    public class GetBillParcelStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class GetBillParcelDate
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("is_present_date")]
        public bool IsPresentDate { get; set; }

        [JsonProperty("is_present_month")]
        public bool IsPresentMonth { get; set; }

        [JsonProperty("is_present_year")]
        public bool IsPresentYear { get; set; }
    }

    #endregion

    #region Cost

    public class GetBillCost
    {
        [JsonProperty("goods")]
        public decimal Goods { get; set; }

        [JsonProperty("shipping")]
        public decimal Shipping { get; set; }

        [JsonProperty("discount")]
        public GetBillCostDiscount Discount { get; set; }

        [JsonProperty("cod")]
        public GetBillCostCod Cod { get; set; }

        [JsonProperty("vat")]
        public GetBillCostVat Vat { get; set; }

        [JsonProperty("gross")]
        public decimal Gross { get; set; }

        [JsonProperty("net")]
        public decimal Net { get; set; }

        [JsonProperty("paid")]
        public decimal Paid { get; set; }

        [JsonProperty("pay_extra")]
        public decimal PayExtra { get; set; }
    }

    public class GetBillCostDiscount
    {
        [JsonProperty("has_discount")]
        public bool HasDiscount { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("percentage")]
        public decimal? Percentage { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }
    }

    public class GetBillCostCod
    {
        [JsonProperty("has_addon")]
        public bool HasAddOn { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("percentage")]
        public decimal? Percentage { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }
    }

    public class GetBillCostVat
    {
        [JsonProperty("has_vat")]
        public bool HasVat { get; set; }

        [JsonProperty("included_vat")]
        public bool IncludedVat { get; set; }

        [JsonProperty("vat_percentage")]
        public decimal VatPercentage { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }
    }

    #endregion

    #region Tag

    public class GetBillTag
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("tag_id")]
        public Guid TagId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    #endregion
}