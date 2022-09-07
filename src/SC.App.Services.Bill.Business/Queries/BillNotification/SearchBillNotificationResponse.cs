using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SC.App.Services.Bill.Business.Enums;

namespace SC.App.Services.Bill.Business.Queries.BillNotification
{
    public class SearchBillNotificationResponse
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
        public List<SearchBillNotificationItem> Items { get; set; }
    }

    public class SearchBillNotificationItem
    {
        [JsonProperty("bill")]
        public SearchBillNotificationBill Bill { get; set; }

        [JsonProperty("notification")]
        public SearchBillNotificationNotification Notification { get; set; }

        [JsonProperty("recipient")]
        public SearchBillNotificationRecipient Recipient { get; set; }

        [JsonProperty("tags")]
        public List<SearchBillNotificationBillTag> Tags { get; set; }
    }

    #region Bill

    public class SearchBillNotificationBill
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

        [JsonProperty("status")]
        public SearchBillNotificationBillStatus Status { get; set; }

        [JsonProperty("created_on")]
        public SearchBillNotificationDate CreatedOn { get; set; }
    }

    public class SearchBillNotificationBillStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    #endregion

    #region Notification

    public class SearchBillNotificationNotification
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public SearchBillNotificationNotificationStatus Status { get; set; }

        [JsonProperty("issue")]
        public SearchBillNotificationNotificationType Issue { get; set; }

        [JsonProperty("before_cancel")]
        public SearchBillNotificationNotificationType BeforeCancel { get; set; }

        [JsonProperty("cancel")]
        public SearchBillNotificationNotificationType Cancel { get; set; }

        [JsonProperty("summary")]
        public SearchBillNotificationNotificationType Summary { get; set; }
    }

    public class SearchBillNotificationNotificationType
    {
        [JsonProperty("is_notified")]
        public bool IsNotified { get; set; }

        [JsonProperty("notified_on")]
        public DateTime? NotifiedOn { get; set; }

        [JsonProperty("can_notify")]
        public bool? CanNotify { get; set; }
    }

    public class SearchBillNotificationNotificationStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    #endregion

    #region Recipient

    public class SearchBillNotificationRecipient
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias_name")]
        public string AliasName { get; set; }

        [JsonProperty("customer")]
        public SearchBillNotificationCustomer Customer { get; set; }
    }

    #endregion

    #region Customer

    public class SearchBillNotificationCustomer
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
        public List<SearchBillNotificationCustomerTag> Tags { get; set; }
    }

    public class SearchBillNotificationCustomerTag
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

    #region Tag

    public class SearchBillNotificationBillTag
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

    public class SearchBillNotificationDate
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
}