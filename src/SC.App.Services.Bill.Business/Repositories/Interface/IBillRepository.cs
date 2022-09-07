using System;
using System.Collections.Generic;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillRepository : IRepository
    {
        Database.Models.Bill GetById(Guid id);

        List<Database.Models.Bill> GetByIds(Guid[] ids);

        Database.Models.Bill GetByKey(string key);

        List<Database.Models.Bill> GetByCustomerIds(Guid channelId, Guid[] customerIds);

        List<Database.Models.Bill> GetLatestByCustomerIds(Guid channelId, Guid[] customerIds);

        Database.Models.Bill GetLatest(Guid channelId, Guid? customerId, Guid? ignoreBillId, DateTime? before);

        List<Database.Models.Bill> Search(Guid channelId, EnumSearchBillStatus status, DateTime begin, DateTime end);

        List<Database.Models.Bill> Search(Guid channelId, EnumSearchBillStatus status, DateTime begin, DateTime end, string keyword, string sortBy, bool sortDesc, int page, int pageSize);

        int Count(Guid channelId, EnumSearchBillStatus status, DateTime begin, DateTime end);

        int Count(Guid channelId, EnumSearchBillStatus status, DateTime begin, DateTime end, string keyword);

        int CountDeposit(Guid channelId, DateTime begin, DateTime end);

        int CountCod(Guid channelId, DateTime begin, DateTime end);

        Guid Add(Database.Models.Bill bill);

        void Update(Database.Models.Bill bill);

        void Updates(List<Database.Models.Bill> bills);
    }
}