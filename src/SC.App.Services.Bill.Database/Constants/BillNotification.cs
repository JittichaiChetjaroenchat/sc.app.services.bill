namespace SC.App.Services.Bill.Database.Constants
{
    public class BillNotification
    {
        public const string TableName = "bill_notifications";

        public static class Column
        {
            public const string Id = "id";

            public const string BillId = "bill_id";

            public const string IsIssueNotified = "is_issue_notified";

            public const string IssueNotifiedOn = "issue_notified_on";

            public const string CanNotifyIssue = "can_notify_issue";

            public const string IsBeforeCancelNotified = "is_before_cancel_notified";

            public const string BeforeCancelNotifiedOn = "before_cancel_notified_on";

            public const string CanNotifyBeforeCancel = "can_notify_before_cancel";

            public const string IsCancelNotified = "is_cancel_notified";

            public const string CancelNotifiedOn = "cancel_notified_on";

            public const string CanNotifyCancel = "can_notify_cancel";

            public const string IsSummaryNotified = "is_summary_notified";

            public const string SummaryNotifiedOn = "summary_notified_on";

            public const string CanNotifySummary = "can_notify_summary";

            public const string CreatedBy = "created_by";

            public const string CreatedOn = "created_on";

            public const string UpdatedBy = "updated_by";

            public const string UpdatedOn = "updated_on";
        }
    }
}