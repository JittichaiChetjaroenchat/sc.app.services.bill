using System;
using SC.App.Services.Bill.Business.Queries.BillNotification;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class BillNotificationMapper
    {
        public static GetBillNotificationResponse Map(BillNotification billNotification)
        {
            if (billNotification == null)
            {
                return null;
            }

            return new GetBillNotificationResponse
            {
                Issue = Map(billNotification.IsIssueNotified, billNotification.IssueNotifiedOn, billNotification.CanNotifyIssue),
                BeforeCancel = Map(billNotification.IsBeforeCancelNotified, billNotification.BeforeCancelNotifiedOn, billNotification.CanNotifyBeforeCancel),
                Cancel = Map(billNotification.IsCancelNotified, billNotification.CancelNotifiedOn, billNotification.CanNotifyCancel),
                Summary = Map(billNotification.IsSummaryNotified, billNotification.SummaryNotifiedOn, billNotification.CanNotifySummary)
            };
        }

        private static GetBillNotificationItem Map(bool isNotified, DateTime? notifiedOn, bool? canNotify)
        {
            return new GetBillNotificationItem
            {
                IsNotified = isNotified,
                NotifiedOn = Map(notifiedOn),
                CanNotify = canNotify
            };
        }

        private static GetBillNotificationDate Map(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }

            return new GetBillNotificationDate
            {
                Date = dateTime.Value,
                IsPresentDate = DateTimeHelper.IsPresentDate(dateTime.Value),
                IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime.Value),
                IsPresentYear = DateTimeHelper.IsPresentYear(dateTime.Value)
            };
        }
    }
}