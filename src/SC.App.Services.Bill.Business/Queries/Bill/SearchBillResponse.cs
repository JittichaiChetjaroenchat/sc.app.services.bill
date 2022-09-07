using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.Bill
{
    public class SearchBillResponse
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("number_of_items")]
        public int NumberOfItems { get; set; }

        [JsonProperty("number_of_pages")]
        public int NumberOfPages { get; set; }

        [JsonProperty("items")]
        public List<SearchBillItem> Items { get; set; }
    }

    public class SearchBillItem
    {
        [JsonProperty("bill")]
        public SearchBill Bill { get; set; }

        [JsonProperty("notification")]
        public SearchBillNotification Notification { get; set; }

        [JsonProperty("recipient")]
        public SearchBillRecipient Recipient { get; set; }

        [JsonProperty("order")]
        public SearchBillOrder Order { get; set; }

        [JsonProperty("payment")]
        public SearchPayment Payment { get; set; }

        [JsonProperty("shipping")]
        public SearchBillShipping Shipping { get; set; }

        [JsonProperty("cost")]
        public SearchBillCost Cost { get; set; }

        [JsonProperty("tags")]
        public List<SearchBillTag> Tags { get; set; }
    }

    #region Bill

    public class SearchBill
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("bill_no")]
        public string BillNo { get; set; }

        [JsonProperty("running_no")]
        public string RunningNo { get; set; }

        [JsonProperty("is_deposit")]
        public bool IsDeposit { get; set; }

        [JsonProperty("is_new_customer")]
        public bool IsNewCustomer { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("discount")]
        public SearchBillDiscount Discount { get; set; }

        [JsonProperty("payment")]
        public SearchBillPayment Payment { get; set; }

        [JsonProperty("status")]
        public SearchBillStatus Status { get; set; }

        [JsonProperty("created_on")]
        public SearchBillDate CreatedOn { get; set; }
    }

    public class SearchBillDiscount
    {
        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("percentage")]
        public decimal? Percentage { get; set; }
    }

    public class SearchBillPayment
    {
        [JsonProperty("type")]
        public SearchBillPaymentType Type { get; set; }

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

    public class SearchBillPaymentType
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class SearchBillStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class SearchBillDate
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

    #region Notification

    public class SearchBillNotification
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public SearchBillNotificationStatus Status { get; set; }
    }

    public class SearchBillNotificationStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    #endregion

    #region Recipient

    public class SearchBillRecipient
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias_name")]
        public string AliasName { get; set; }

        [JsonProperty("contact")]
        public SearchBillRecipientContact Contact { get; set; }

        [JsonProperty("customer")]
        public SearchBillCustomer Customer { get; set; }
    }

    public class SearchBillRecipientContact
    {
        [JsonProperty("full_address")]
        public string FullAddress { get; set; }

        [JsonProperty("primary_phone")]
        public string PrimaryPhone { get; set; }

        [JsonProperty("secondary_phone")]
        public string SecondaryPhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class SearchBillCustomer
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_new")]
        public bool IsNew { get; set; }

        [JsonProperty("is_blocked")]
        public bool IsBlocked { get; set; }

        [JsonProperty("tags")]
        public List<SearchBillCustomerTag> Tags { get; set; }
    }

    public class SearchBillCustomerTag
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

    public class SearchBillOrder
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }

    #endregion

    #region Payment

    public class SearchPayment
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("status")]
        public SearchPaymentStatus Status { get; set; }

        [JsonProperty("latest")]
        public SearchPaymentLatest Latest { get; set; }

        [JsonProperty("accepted")]
        public SearchPaymentAccepted Accepted { get; set; }
    }

    public class SearchPaymentStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class SearchPaymentLatest
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("has_evidence")]
        public bool HasEvidence { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("pay_on")]
        public SearchPaymentDate Payon { get; set; }

        [JsonProperty("verification")]
        public SearchPaymentVerification Verification { get; set; }
    }

    public class SearchPaymentDate
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

    public class SearchPaymentVerification
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("status")]
        public SearchPaymentVerificationStatus Status { get; set; }
    }

    public class SearchPaymentVerificationStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class SearchPaymentAccepted
    {
        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }

    #endregion

    #region Shipping

    public class SearchBillShipping
    {
        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        [JsonProperty("parcels")]
        public List<SearchBillParcel> Parcels { get; set; }
    }

    #endregion

    #region Parcel

    public class SearchBillParcel
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
        public SearchBillParcelStatus Status { get; set; }

        [JsonProperty("created_on")]
        public SearchBillParcelDate CreatedOn { get; set; }

        [JsonProperty("is_printed")]
        public bool IsPrinted { get; set; }

        [JsonProperty("is_packed")]
        public bool IsPacked { get; set; }
    }

    public class SearchBillParcelStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class SearchBillParcelDate
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

    public class SearchBillCost
    {
        [JsonProperty("goods")]
        public decimal Goods { get; set; }

        [JsonProperty("shipping")]
        public decimal Shipping { get; set; }

        [JsonProperty("discount")]
        public SearchBillCostDiscount Discount { get; set; }

        [JsonProperty("cod")]
        public SearchBillCostCod Cod { get; set; }

        [JsonProperty("vat")]
        public SearchBillCostVat Vat { get; set; }

        [JsonProperty("gross")]
        public decimal Gross { get; set; }

        [JsonProperty("net")]
        public decimal Net { get; set; }

        [JsonProperty("paid")]
        public decimal Paid { get; set; }

        [JsonProperty("pay_extra")]
        public decimal PayExtra { get; set; }
    }

    public class SearchBillCostDiscount
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

    public class SearchBillCostCod
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

    public class SearchBillCostVat
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

    public class SearchBillTag
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