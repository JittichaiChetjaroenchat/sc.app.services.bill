using System;
using Newtonsoft.Json;

namespace SC.App.Services.Bill.Business.Queries.BillNotification
{
    public class GetBillNotificationResponse
    {
        [JsonProperty("issue")]
        public GetBillNotificationItem Issue { get; set; }

        [JsonProperty("before_cancel")]
        public GetBillNotificationItem BeforeCancel { get; set; }

        [JsonProperty("cancel")]
        public GetBillNotificationItem Cancel { get; set; }

        [JsonProperty("summary")]
        public GetBillNotificationItem Summary { get; set; }
    }

    public class GetBillNotificationItem
    {
        [JsonProperty("is_notified")]
        public bool IsNotified { get; set; }

        [JsonProperty("notified_on")]
        public GetBillNotificationDate NotifiedOn { get; set; }

        [JsonProperty("can_notify")]
        public bool? CanNotify { get; set; }
    }

    public class GetBillNotificationDate
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