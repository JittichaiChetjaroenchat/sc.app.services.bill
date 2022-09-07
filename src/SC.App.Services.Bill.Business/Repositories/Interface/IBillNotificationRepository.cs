using System;
using System.Collections.Generic;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillNotificationRepository : IRepository
    {
        Database.Models.BillNotification GetById(Guid id);

        Database.Models.BillNotification GetByBillId(Guid billId);

        Database.Models.BillNotification GetLatest(Guid channelId, Guid? customerId, Guid? ignoreBillId, DateTime? before);

        List<Database.Models.BillNotification> Search(Guid channelId, EnumSearchBillNotificationStatus status, DateTime begin, DateTime end, string keyword, string sortBy, bool sortDesc, int page, int pageSize);

        int Count(Guid channelId, EnumSearchBillNotificationStatus status, DateTime begin, DateTime end);

        int Count(Guid channelId, EnumSearchBillNotificationStatus status, DateTime begin, DateTime end, string keyword);

        Guid Add(Database.Models.BillNotification billNotification);

        void Update(Database.Models.BillNotification billNotification);
    }
}