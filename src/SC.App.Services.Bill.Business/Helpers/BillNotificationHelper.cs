using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Queries.BillNotification;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class BillNotificationHelper
    {
        public static List<SearchBillNotificationItem> Search(List<SearchBillNotificationItem> items, string keyword)
        {
            if (keyword.IsEmpty())
            {
                return items;
            }

            return items
                .Where(x =>
                    x.Bill != null && !x.Bill.RunningNo.IsEmpty() && x.Bill.RunningNo.ToLower().Contains(keyword.ToLower()) ||
                    x.Recipient != null && !x.Recipient.Name.IsEmpty() && x.Recipient.Name.ToLower().Contains(keyword.ToLower()) ||
                    x.Recipient != null && x.Recipient.Customer != null && x.Recipient.Customer.Tags.Any(x => x.Name.ToLower().Contains(keyword.ToLower())) ||
                    x.Tags.Any(x => x.Name.ToLower().Contains(keyword.ToLower())))
                .ToList();
        }

        public static List<SearchBillNotificationItem> Sort(List<SearchBillNotificationItem> items, string sortBy, bool sortDesc)
        {
            if (sortBy.IsEmpty())
            {
                return items;
            }

            switch (sortBy)
            {
                default:
                    return items;
            }
        }

        public static EnumBillNotificationStatus GetStatus(BillNotification notification)
        {
            if (notification == null)
            {
                return EnumBillNotificationStatus.Unknown;
            }

            var hasIssueProblem = HasProblem(notification.IsIssueNotified, notification.CanNotifyIssue);
            var hasBeforeCancelProblem = HasProblem(notification.IsBeforeCancelNotified, notification.CanNotifyBeforeCancel);
            var hasCancelProblem = HasProblem(notification.IsCancelNotified, notification.CanNotifyCancel);
            var hasSummaryProblem = HasProblem(notification.IsSummaryNotified, notification.CanNotifySummary);

            var items = new List<bool>() { hasIssueProblem, hasBeforeCancelProblem, hasCancelProblem, hasSummaryProblem };
            var isAllSuccess = items.All(x => x == false);
            if (isAllSuccess)
            {
                return EnumBillNotificationStatus.Success;
            }

            var isAnyProblem = items.Any(x => x == true);
            if (isAnyProblem)
            {
                return EnumBillNotificationStatus.Warning;
            }
            
            var isAllError = items.All(x => x == true);
            if (isAllError)
            {
                return EnumBillNotificationStatus.Error;
            }

            return EnumBillNotificationStatus.Unknown;
        }

        private static bool HasProblem(bool isNotified, bool? canNotify)
        {
            if (!isNotified)
            {
                return false;
            }

            if (!canNotify.HasValue)
            {
                return false;
            }

            return !canNotify.Value;
        }
    }
}