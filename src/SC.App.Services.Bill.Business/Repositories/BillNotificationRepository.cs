using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillNotificationRepository : BaseRepository, IBillNotificationRepository
    {
        public BillNotificationRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillNotification GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillNotifications
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public BillNotification GetByBillId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillNotifications
                    .FirstOrDefault(x => x.BillId == billId);
            }
        }

        public BillNotification GetLatest(Guid channelId, Guid? customerId, Guid? ignoreBillId, DateTime? before)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumBillStatus.Unknown.GetDescription(), EnumBillStatus.Deleted.GetDescription() };
                var query = context.BillNotifications
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillRecipient)
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillStatus)
                    .OrderByDescending(x => x.CreatedOn)
                    .Where(x => x.Bill != null && x.Bill.ChannelId == channelId && !excludeStatuses.Contains(x.Bill.BillStatus.Code));

                if (customerId.HasValue)
                {
                    query = query.Where(x => x.Bill != null && x.Bill.BillRecipient != null && x.Bill.BillRecipient.CustomerId == customerId.Value);
                }

                if (ignoreBillId.HasValue)
                {
                    query = query.Where(x => x.Id != ignoreBillId.Value);
                }

                if (before.HasValue)
                {
                    query = query.Where(x => x.CreatedOn <= before.Value);
                }

                return query.FirstOrDefault();
            }
        }

        public List<BillNotification> Search(Guid channelId, EnumSearchBillNotificationStatus status, DateTime begin, DateTime end, string keyword, string sortBy, bool sortDesc, int page, int pageSize)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var query = context.BillNotifications
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillRecipient)
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillStatus)
                    .Where(x => x.Bill != null && x.Bill.ChannelId == channelId)
                    .Where(x => x.Bill != null && x.Bill.BillStatus != null && x.Bill.BillStatus.Code == EnumBillStatus.Pending.GetDescription());

                // Status
                if (status == EnumSearchBillNotificationStatus.Unknown)
                {
                    // Skip
                }
                else if (status == EnumSearchBillNotificationStatus.SentSummary)
                {
                    query = query
                        .Where(x => x.IsSummaryNotified && x.CanNotifySummary.HasValue && x.CanNotifySummary.Value);
                }
                else if (status == EnumSearchBillNotificationStatus.UnsentSummary)
                {
                    query = query
                        .Where(x => !x.IsSummaryNotified || (x.IsSummaryNotified && x.CanNotifySummary.HasValue && !x.CanNotifySummary.Value));
                }

                // Search
                if (!keyword.IsEmpty())
                {
                    query = query
                    .Where(x =>
                        x.Bill != null &&
                        x.Bill.RunningNo != null && x.Bill.RunningNo.Contains(keyword) ||
                        x.Bill.BillRecipient != null && x.Bill.BillRecipient.Name != null && x.Bill.BillRecipient.Name.Contains(keyword) ||
                        x.Bill.BillTags.Any(x => x.Tag != null && x.Tag.Name.Contains(keyword)));
                }

                // Dates
                query = query
                    .Where(x => x.CreatedOn.Date >= begin && x.CreatedOn.Date <= end);

                // Sorting
                if (!sortBy.IsEmpty())
                {
                    switch (sortBy)
                    {
                        case "bill.running_no":
                            query = sortDesc ? query.OrderByDescending(x => x.Bill.RunningNo) : query.OrderBy(x => x.Bill.RunningNo);
                            break;
                        case "bill.created_on":
                            query = sortDesc ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn);
                            break;
                        case "bill.updated_on":
                            query = sortDesc ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn);
                            break;
                        default:
                            query = query.OrderByDescending(x => x.CreatedOn);
                            break;
                    }
                }
                else
                {
                    query = query.OrderByDescending(x => x.CreatedOn);
                }

                // Paging
                query = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                return query.ToList();
            }
        }

        public int Count(Guid channelId, EnumSearchBillNotificationStatus status, DateTime begin, DateTime end)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var query = context.BillNotifications
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillRecipient)
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillStatus)
                    .Where(x => x.Bill != null && x.Bill.ChannelId == channelId)
                    .Where(x => x.Bill != null && x.Bill.BillStatus != null && x.Bill.BillStatus.Code == EnumBillStatus.Pending.GetDescription());

                // Status
                if (status == EnumSearchBillNotificationStatus.Unknown)
                {
                    // Skip
                }
                else if (status == EnumSearchBillNotificationStatus.SentSummary)
                {
                    query = query
                        .Where(x => x.IsSummaryNotified && x.CanNotifySummary.HasValue && x.CanNotifySummary.Value);
                }
                else if (status == EnumSearchBillNotificationStatus.UnsentSummary)
                {
                    query = query
                        .Where(x => !x.IsSummaryNotified || (x.IsSummaryNotified && x.CanNotifySummary.HasValue && !x.CanNotifySummary.Value));
                }

                // Dates
                query = query
                    .Where(x => x.CreatedOn.Date >= begin && x.CreatedOn.Date <= end);

                return query.Count();
            }
        }

        public int Count(Guid channelId, EnumSearchBillNotificationStatus status, DateTime begin, DateTime end, string keyword)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var query = context.BillNotifications
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillRecipient)
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.Bill)
                    .ThenInclude(x => x.BillStatus)
                    .Where(x => x.Bill != null && x.Bill.ChannelId == channelId)
                    .Where(x => x.Bill != null && x.Bill.BillStatus != null && x.Bill.BillStatus.Code == EnumBillStatus.Pending.GetDescription());

                // Status
                if (status == EnumSearchBillNotificationStatus.Unknown)
                {
                    // Skip
                }
                else if (status == EnumSearchBillNotificationStatus.SentSummary)
                {
                    query = query
                        .Where(x => x.IsSummaryNotified && x.CanNotifySummary.HasValue && x.CanNotifySummary.Value);
                }
                else if (status == EnumSearchBillNotificationStatus.UnsentSummary)
                {
                    query = query
                        .Where(x => !x.IsSummaryNotified || (x.IsSummaryNotified && x.CanNotifySummary.HasValue && !x.CanNotifySummary.Value));
                }

                // Search
                if (!keyword.IsEmpty())
                {
                    query = query
                        .Where(x =>
                            x.Bill != null &&
                            x.Bill.RunningNo != null && x.Bill.RunningNo.Contains(keyword) ||
                            x.Bill.BillRecipient != null && x.Bill.BillRecipient.Name != null && x.Bill.BillRecipient.Name.Contains(keyword) ||
                            x.Bill.BillTags.Any(x => x.Tag != null && x.Tag.Name.Contains(keyword)));
                }

                // Dates
                query = query
                    .Where(x => x.CreatedOn.Date >= begin && x.CreatedOn.Date <= end);

                return query.Count();
            }
        }

        public Guid Add(BillNotification billNotification)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billNotification);
                context.SaveChanges();

                return billNotification.Id;
            }
        }        

        public void Update(BillNotification billNotification)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(billNotification).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}