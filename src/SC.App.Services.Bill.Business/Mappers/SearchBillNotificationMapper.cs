using System;
using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Helpers;
using SC.App.Services.Bill.Business.Queries.BillNotification;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class SearchBillNotificationMapper
    {
        public static SearchBillNotificationResponse Map(int page, int pageSize, int numberOfItems, int numberOfPages, List<SearchBillNotificationItem> items)
        {
            return new SearchBillNotificationResponse
            {
                Page = page,
                PageSize = pageSize,
                NumberOfItems = numberOfItems,
                NumberOfPages = numberOfPages,
                Items = items
            };
        }

        public static SearchBillNotificationItem Map(
            string baseUrl,
            BillNotification billNotification,
            Customer.Client.GetCustomerResponse customer,
            Setting.Client.GetPreferencesResponse preferences)
        {
            return new SearchBillNotificationItem
            {
                Bill = BillMapper.Map(baseUrl, billNotification.Bill, billNotification.Bill.BillStatus, preferences),
                Notification = NotificationMapper.Map(billNotification),
                Recipient = RecipientMapper.Map(billNotification.Bill.BillRecipient, customer),
                Tags = BillTagMapper.Map(billNotification.Bill.BillTags)
            };
        }

        private class BillMapper
        {
            public static SearchBillNotificationBill Map(string baseUrl, Database.Models.Bill bill, BillStatus status, Setting.Client.GetPreferencesResponse preferences)
            {
                return new SearchBillNotificationBill
                {
                    Id = bill.Id,
                    BillNo = bill.BillNo,
                    RunningNo = bill.RunningNo,
                    IsDeposit = bill.IsDeposit,
                    IsNewCustomer = bill.IsNewCustomer,
                    Link = BillHelper.GetLink(baseUrl, bill.Key, preferences.Language),
                    Status = Map(status),
                    CreatedOn = Map(bill.CreatedOn)
                };
            }

            private static SearchBillNotificationBillStatus Map(BillStatus status)
            {
                return new SearchBillNotificationBillStatus
                {
                    Code = status.Code
                };
            }

            private static SearchBillNotificationDate Map(DateTime dateTime)
            {
                return new SearchBillNotificationDate
                {
                    Date = dateTime,
                    IsPresentDate = DateTimeHelper.IsPresentDate(dateTime),
                    IsPresentMonth = DateTimeHelper.IsPresentMonth(dateTime),
                    IsPresentYear = DateTimeHelper.IsPresentYear(dateTime)
                };
            }
        }

        private class NotificationMapper
        {
            public static SearchBillNotificationNotification Map(BillNotification notification)
            {
                if (notification == null)
                {
                    return null;
                }

                var status = BillNotificationHelper.GetStatus(notification);

                return new SearchBillNotificationNotification
                {
                    Id = notification.Id,
                    Status = Map(status),
                    Issue = Map(notification.IsIssueNotified, notification.IssueNotifiedOn, notification.CanNotifyIssue),
                    BeforeCancel = Map(notification.IsBeforeCancelNotified, notification.BeforeCancelNotifiedOn, notification.CanNotifyBeforeCancel),
                    Cancel = Map(notification.IsCancelNotified, notification.CancelNotifiedOn, notification.CanNotifyCancel),
                    Summary = Map(notification.IsSummaryNotified, notification.SummaryNotifiedOn, notification.CanNotifySummary)
                };
            }

            private static SearchBillNotificationNotificationType Map(bool isNotified, DateTime? notifiedOn, bool? canNotify)
            {
                return new SearchBillNotificationNotificationType
                {
                    IsNotified = isNotified,
                    NotifiedOn = notifiedOn,
                    CanNotify = canNotify
                };
            }

            private static SearchBillNotificationNotificationStatus Map(EnumBillNotificationStatus status)
            {
                return new SearchBillNotificationNotificationStatus
                {
                    Code = status.GetDescription()
                };
            }
        }

        private class RecipientMapper
        {
            public static SearchBillNotificationRecipient Map(BillRecipient recipient, Customer.Client.GetCustomerResponse customer)
            {
                return new SearchBillNotificationRecipient
                {
                    Id = recipient.Id,
                    AliasName = recipient.AliasName,
                    Name = recipient.Name,
                    Customer = CustomerMapper.Map(customer)
                };
            }
        }

        private class CustomerMapper
        {
            public static SearchBillNotificationCustomer Map(Customer.Client.GetCustomerResponse customer)
            {
                if (customer == null)
                {
                    return null;
                }

                return new SearchBillNotificationCustomer
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    IsNew = customer.Is_new,
                    IsBlocked = customer.Is_blocked,
                    Tags = CustomerTagMapper.Map(customer.Tags)
                };
            }
        }

        private class CustomerTagMapper
        {
            public static List<SearchBillNotificationCustomerTag> Map(ICollection<Customer.Client.GetCustomerTag> customerTags)
            {
                return customerTags
                    .Select(Map)
                    .ToList();
            }

            public static SearchBillNotificationCustomerTag Map(Customer.Client.GetCustomerTag customerTag)
            {
                if (customerTag == null)
                {
                    return null;
                }

                return new SearchBillNotificationCustomerTag
                {
                    Id = customerTag.Id,
                    TagId = customerTag.Tag_id,
                    Color = customerTag.Color,
                    Name = customerTag.Name,
                    Description = customerTag.Description
                };
            }
        }

        private class BillTagMapper
        {
            public static List<SearchBillNotificationBillTag> Map(ICollection<BillTag> billTags)
            {
                return billTags
                    .Select(Map)
                    .ToList();
            }

            private static SearchBillNotificationBillTag Map(BillTag billTag)
            {
                if (billTag == null || billTag.Tag == null)
                {
                    return null;
                }

                return new SearchBillNotificationBillTag
                {
                    Id = billTag.Id,
                    TagId = billTag.Tag.Id,
                    Name = billTag.Tag.Name,
                    Color = billTag.Tag.Color,
                    Description = billTag.Tag.Description
                };
            }
        }
    }
}