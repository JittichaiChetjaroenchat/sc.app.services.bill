using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillRepository : BaseRepository, IBillRepository
    {
        public BillRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public Database.Models.Bill GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumBillStatus.Unknown.GetDescription(), EnumBillStatus.Deleted.GetDescription() };
                return context.Bills
                    .Include(x => x.BillChannel)
                    .Include(x => x.BillDiscount)
                    .Include(x => x.BillNotification)
                    .Include(x => x.BillRecipient)
                    .ThenInclude(x => x.BillRecipientContact)
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingRangeRule)
                    .ThenInclude(x => x.BillShippingRanges)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingTotalRule)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingFreeRule)
                    .Include(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.BillStatus)
                    .Include(x => x.Parcels)
                    .ThenInclude(x => x.ParcelStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentVerification)
                    .ThenInclude(x => x.PaymentVerificationStatus)
                    .FirstOrDefault(x => x.Id == id && !excludeStatuses.Contains(x.BillStatus.Code));
            }
        }

        public List<Database.Models.Bill> GetByIds(Guid[] ids)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumBillStatus.Unknown.GetDescription(), EnumBillStatus.Deleted.GetDescription() };
                return context.Bills
                    .Include(x => x.BillChannel)
                    .Include(x => x.BillDiscount)
                    .Include(x => x.BillNotification)
                    .Include(x => x.BillRecipient)
                    .ThenInclude(x => x.BillRecipientContact)
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingRangeRule)
                    .ThenInclude(x => x.BillShippingRanges)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingTotalRule)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingFreeRule)
                    .Include(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.BillStatus)
                    .Include(x => x.Parcels)
                    .ThenInclude(x => x.ParcelStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentVerification)
                    .ThenInclude(x => x.PaymentVerificationStatus)
                    .Where(x => ids.Contains(x.Id) && !excludeStatuses.Contains(x.BillStatus.Code))
                    .ToList();
            }
        }

        public Database.Models.Bill GetByKey(string key)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumBillStatus.Unknown.GetDescription(), EnumBillStatus.Deleted.GetDescription() };
                return context.Bills
                    .Include(x => x.BillChannel)
                    .Include(x => x.BillDiscount)
                    .Include(x => x.BillNotification)
                    .Include(x => x.BillRecipient)
                    .ThenInclude(x => x.BillRecipientContact)
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingRangeRule)
                    .ThenInclude(x => x.BillShippingRanges)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingTotalRule)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingFreeRule)
                    .Include(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.BillStatus)
                    .Include(x => x.Parcels)
                    .ThenInclude(x => x.ParcelStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentVerification)
                    .ThenInclude(x => x.PaymentVerificationStatus)
                    .FirstOrDefault(x => x.Key == key && !excludeStatuses.Contains(x.BillStatus.Code));
            }
        }

        public List<Database.Models.Bill> GetByCustomerIds(Guid channelId, Guid[] customerIds)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumBillStatus.Deleted.GetDescription() };
                var query = context.Bills
                    .Include(x => x.BillChannel)
                    .Include(x => x.BillDiscount)
                    .Include(x => x.BillNotification)
                    .Include(x => x.BillRecipient)
                    .ThenInclude(x => x.BillRecipientContact)
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingRangeRule)
                    .ThenInclude(x => x.BillShippingRanges)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingTotalRule)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingFreeRule)
                    .Include(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.BillStatus)
                    .Include(x => x.Parcels)
                    .ThenInclude(x => x.ParcelStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentVerification)
                    .ThenInclude(x => x.PaymentVerificationStatus)
                    .Where(x => x.ChannelId == channelId && !excludeStatuses.Contains(x.BillStatus.Code));

                query = query
                    .Where(x => customerIds.Contains(x.BillRecipient.CustomerId));

                return query.ToList();
            }
        }

        public Database.Models.Bill GetLatest(Guid channelId, Guid? customerId, Guid? ignoreBillId, DateTime? before)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumBillStatus.Unknown.GetDescription(), EnumBillStatus.Deleted.GetDescription() };
                var query = context.Bills
                    .Include(x => x.BillChannel)
                    .Include(x => x.BillDiscount)
                    .Include(x => x.BillNotification)
                    .Include(x => x.BillRecipient)
                    .ThenInclude(x => x.BillRecipientContact)
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingRangeRule)
                    .ThenInclude(x => x.BillShippingRanges)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingTotalRule)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingFreeRule)
                    .Include(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.BillStatus)
                    .Include(x => x.Parcels)
                    .ThenInclude(x => x.ParcelStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentVerification)
                    .ThenInclude(x => x.PaymentVerificationStatus)
                    .OrderByDescending(x => x.CreatedOn)
                    .Where(x => x.ChannelId == channelId && !excludeStatuses.Contains(x.BillStatus.Code));

                if (customerId.HasValue)
                {
                    query = query.Where(x => x.BillRecipient != null && x.BillRecipient.CustomerId == customerId.Value);
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

        public List<Database.Models.Bill> GetLatestByCustomerIds(Guid channelId, Guid[] customerIds)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumBillStatus.Unknown.GetDescription(), EnumBillStatus.Deleted.GetDescription() };
                var query = context.Bills
                    .Include(x => x.BillChannel)
                    .Include(x => x.BillDiscount)
                    .Include(x => x.BillNotification)
                    .Include(x => x.BillRecipient)
                    .ThenInclude(x => x.BillRecipientContact)
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingRangeRule)
                    .ThenInclude(x => x.BillShippingRanges)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingTotalRule)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingFreeRule)
                    .Include(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.BillStatus)
                    .Include(x => x.Parcels)
                    .ThenInclude(x => x.ParcelStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentVerification)
                    .ThenInclude(x => x.PaymentVerificationStatus)
                    .OrderByDescending(x => x.CreatedOn)
                    .Where(x => x.ChannelId == channelId && !excludeStatuses.Contains(x.BillStatus.Code));

                query = query
                    .Where(x => customerIds.Contains(x.BillRecipient.CustomerId));

                return query
                    .AsEnumerable()
                    .OrderByDescending(x => x.CreatedOn)
                    .GroupBy(x => x.BillRecipient.CustomerId)
                    .Select(x => x.First())
                    .ToList();
            }
        }

        public List<Database.Models.Bill> Search(Guid channelId, EnumSearchBillStatus status, DateTime begin, DateTime end)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumSearchBillStatus.Deleted.GetDescription() };
                var query = context.Bills
                    .Include(x => x.BillChannel)
                    .Include(x => x.BillDiscount)
                    .Include(x => x.BillNotification)
                    .Include(x => x.BillRecipient)
                    .ThenInclude(x => x.BillRecipientContact)
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingRangeRule)
                    .ThenInclude(x => x.BillShippingRanges)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingTotalRule)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingFreeRule)
                    .Include(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.BillStatus)
                    .Include(x => x.Parcels)
                    .ThenInclude(x => x.ParcelStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentVerification)
                    .ThenInclude(x => x.PaymentVerificationStatus)
                    .Where(x => x.ChannelId == channelId && !excludeStatuses.Contains(x.BillStatus.Code));

                // Status
                if (status == EnumSearchBillStatus.Unknown)
                {
                    // Skip
                }
                else if (status == EnumSearchBillStatus.Deposited)
                {
                    query = query
                        .Where(x => x.IsDeposit);
                }
                else if (status == EnumSearchBillStatus.Cod)
                {
                    query = query
                        .Where(x => x.BillPayment != null && x.BillPayment.BillPaymentType != null && x.BillPayment.BillPaymentType.Code.Equals(EnumBillPaymentType.PostPaid.GetDescription()));
                }
                else if (status == EnumSearchBillStatus.Printing)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => !x.Parcels.Any(x => x.IsPrinted));
                }
                else if (status == EnumSearchBillStatus.Printed)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.Any(x => x.IsPrinted));
                }
                else if (status == EnumSearchBillStatus.Packing)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.All(x => x.IsPrinted) && x.Parcels.All(x => !x.IsPacked));
                }
                else if (status == EnumSearchBillStatus.Packed)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.All(x => x.IsPrinted) && x.Parcels.Any(x => x.IsPacked));
                }
                else
                {
                    query = query
                        .Where(x => !x.IsDeposit)
                        .Where(x => x.BillStatus.Code == status.GetDescription());
                }

                // Dates
                query = query
                    .Where(x => x.CreatedOn.Date >= begin && x.CreatedOn.Date <= end);

                return query.ToList();
            }
        }

        public List<Database.Models.Bill> Search(Guid channelId, EnumSearchBillStatus status, DateTime begin, DateTime end, string keyword, string sortBy, bool sortDesc, int page, int pageSize)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumSearchBillStatus.Deleted.GetDescription() };
                var query = context.Bills
                    .Include(x => x.BillChannel)
                    .Include(x => x.BillDiscount)
                    .Include(x => x.BillNotification)
                    .Include(x => x.BillRecipient)
                    .ThenInclude(x => x.BillRecipientContact)
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingRangeRule)
                    .ThenInclude(x => x.BillShippingRanges)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingTotalRule)
                    .Include(x => x.BillShipping)
                    .ThenInclude(x => x.BillShippingFreeRule)
                    .Include(x => x.BillTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.BillStatus)
                    .Include(x => x.Parcels)
                    .ThenInclude(x => x.ParcelStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentStatus)
                    .Include(x => x.Payments)
                    .ThenInclude(x => x.PaymentVerification)
                    .ThenInclude(x => x.PaymentVerificationStatus)
                    .Where(x => x.ChannelId == channelId && !excludeStatuses.Contains(x.BillStatus.Code));

                // Status
                if (status == EnumSearchBillStatus.Unknown)
                {
                    // Skip
                }
                else if (status == EnumSearchBillStatus.Deposited)
                {
                    query = query
                        .Where(x => x.IsDeposit);
                }
                else if (status == EnumSearchBillStatus.Cod)
                {
                    query = query
                        .Where(x => x.BillPayment != null && x.BillPayment.BillPaymentType != null && x.BillPayment.BillPaymentType.Code.Equals(EnumBillPaymentType.PostPaid.GetDescription()));
                }
                else if (status == EnumSearchBillStatus.Printing)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => !x.Parcels.Any(a => a.IsPrinted));
                }
                else if (status == EnumSearchBillStatus.Printed)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.Any(a => a.IsPrinted));
                }
                else if (status == EnumSearchBillStatus.Packing)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.All(x => x.IsPrinted) && x.Parcels.All(x => !x.IsPacked));
                }
                else if (status == EnumSearchBillStatus.Packed)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.All(x => x.IsPrinted) && x.Parcels.Any(x => x.IsPacked));
                }
                else
                {
                    query = query
                        .Where(x => !x.IsDeposit)
                        .Where(x => x.BillStatus.Code == status.GetDescription());
                }

                // Search
                if (!keyword.IsEmpty())
                {
                    query = query
                    .Where(x =>
                        x.RunningNo != null && x.RunningNo.Contains(keyword) ||
                        x.BillRecipient != null && x.BillRecipient.Name != null && x.BillRecipient.Name.Contains(keyword) ||
                        x.BillTags.Any(x => x.Tag != null && x.Tag.Name.Contains(keyword)));
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
                            query = sortDesc ? query.OrderByDescending(x => x.RunningNo) : query.OrderBy(x => x.RunningNo);
                            break;
                        case "payment.amount":
                            query = sortDesc ?
                                query.OrderByDescending(x => x.Payments.OrderByDescending(o => o.PaymentNo).FirstOrDefault().Amount) :
                                query.OrderBy(x => x.Payments.OrderByDescending(o => o.PaymentNo).FirstOrDefault().Amount);
                            break;
                        case "payment.pay_on":
                            query = sortDesc ?
                                query.OrderByDescending(x => x.Payments.OrderByDescending(o => o.PaymentNo).FirstOrDefault().PayOn) :
                                query.OrderBy(x => x.Payments.OrderByDescending(o => o.PaymentNo).FirstOrDefault().PayOn);
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

        public int Count(Guid channelId, EnumSearchBillStatus status, DateTime begin, DateTime end)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumSearchBillStatus.Deleted.GetDescription() };
                var query = context.Bills
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillStatus)
                    .Where(x => x.ChannelId == channelId && !excludeStatuses.Contains(x.BillStatus.Code));

                // Status
                if (status == EnumSearchBillStatus.Unknown)
                {
                    // Skip
                }
                else if (status == EnumSearchBillStatus.Deposited)
                {
                    query = query
                        .Where(x => x.IsDeposit);
                }
                else if (status == EnumSearchBillStatus.Printing)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => !x.Parcels.Any(a => a.IsPrinted));
                }
                else if (status == EnumSearchBillStatus.Printed)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.Any(a => a.IsPrinted));
                }
                else if (status == EnumSearchBillStatus.Packing)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.All(x => x.IsPrinted) && x.Parcels.All(x => !x.IsPacked));
                }
                else if (status == EnumSearchBillStatus.Packed)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.All(x => x.IsPrinted) && x.Parcels.Any(x => x.IsPacked));
                }
                else
                {
                    query = query
                        .Where(x => !x.IsDeposit)
                        .Where(x => x.BillStatus.Code == status.GetDescription());
                }

                // Dates
                query = query
                    .Where(x => x.CreatedOn.Date >= begin && x.CreatedOn.Date <= end);

                return query.Count();
            }
        }

        public int Count(Guid channelId, EnumSearchBillStatus status, DateTime begin, DateTime end, string keyword)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumSearchBillStatus.Deleted.GetDescription() };
                var query = context.Bills
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillStatus)
                    .Where(x => x.ChannelId == channelId && !excludeStatuses.Contains(x.BillStatus.Code));

                // Status
                if (status == EnumSearchBillStatus.Unknown)
                {
                    // Skip
                }
                else if (status == EnumSearchBillStatus.Deposited)
                {
                    query = query
                        .Where(x => x.IsDeposit);
                }
                else if (status == EnumSearchBillStatus.Cod)
                {
                    query = query
                        .Where(x => x.BillPayment != null && x.BillPayment.BillPaymentType != null && x.BillPayment.BillPaymentType.Code.Equals(EnumBillPaymentType.PostPaid.GetDescription()));
                }
                else if (status == EnumSearchBillStatus.Printing)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => !x.Parcels.Any(a => a.IsPrinted));
                }
                else if (status == EnumSearchBillStatus.Printed)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.Any(a => a.IsPrinted));
                }
                else if (status == EnumSearchBillStatus.Packing)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.All(x => x.IsPrinted) && x.Parcels.All(x => !x.IsPacked));
                }
                else if (status == EnumSearchBillStatus.Packed)
                {
                    query = query
                        .Where(x => x.BillStatus.Code == EnumSearchBillStatus.Done.GetDescription())
                        .Where(x => x.Parcels.All(x => x.IsPrinted) && x.Parcels.Any(x => x.IsPacked));
                }
                else
                {
                    query = query
                        .Where(x => !x.IsDeposit)
                        .Where(x => x.BillStatus.Code == status.GetDescription());
                }

                // Search
                if (!keyword.IsEmpty())
                {
                    query = query
                        .Where(x =>
                            x.RunningNo != null && x.RunningNo.Contains(keyword) ||
                            x.BillRecipient != null && x.BillRecipient.Name != null && x.BillRecipient.Name.Contains(keyword) ||
                            x.BillTags.Any(x => x.Tag != null && x.Tag.Name.Contains(keyword)));
                }

                // Dates
                query = query
                    .Where(x => x.CreatedOn.Date >= begin && x.CreatedOn.Date <= end);

                return query.Count();
            }
        }

        public int CountDeposit(Guid channelId, DateTime begin, DateTime end)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumBillStatus.Deleted.GetDescription() };
                var query = context.Bills
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillStatus)
                    .Where(x => x.ChannelId == channelId)
                    .Where(x => x.IsDeposit)
                    .Where(x => !excludeStatuses.Contains(x.BillStatus.Code));

                // Statuses
                var statuses = new string[] { EnumBillStatus.Done.GetDescription(), EnumBillStatus.Archived.GetDescription() };
                query = query
                    .Where(x => !statuses.Contains(x.BillStatus.Code));

                // Dates
                query = query
                    .Where(x => x.CreatedOn.Date >= begin && x.CreatedOn.Date <= end);

                return query.Count();
            }
        }

        public int CountCod(Guid channelId, DateTime begin, DateTime end)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                var excludeStatuses = new string[] { EnumBillStatus.Deleted.GetDescription() };
                var query = context.Bills
                    .Include(x => x.BillPayment)
                    .ThenInclude(x => x.BillPaymentType)
                    .Include(x => x.BillStatus)
                    .Where(x => x.ChannelId == channelId)
                    .Where(x => x.BillPayment != null && x.BillPayment.BillPaymentType != null && x.BillPayment.BillPaymentType.Code.Equals(EnumBillPaymentType.PostPaid.GetDescription()))
                    .Where(x => !excludeStatuses.Contains(x.BillStatus.Code));

                // Dates
                query = query
                    .Where(x => x.CreatedOn.Date >= begin && x.CreatedOn.Date <= end);

                return query.Count();
            }
        }

        public Guid Add(Database.Models.Bill bill)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(bill);
                context.SaveChanges();

                return bill.Id;
            }
        }

        public void Update(Database.Models.Bill bill)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(bill).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Updates(List<Database.Models.Bill> bills)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                foreach (var bill in bills)
                {
                    context.Entry(bill).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
        }
    }
}