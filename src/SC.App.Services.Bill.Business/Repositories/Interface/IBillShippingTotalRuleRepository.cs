using System;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillShippingTotalRuleRepository : IRepository
    {
        Database.Models.BillShippingTotalRule GetByBillShippingId(Guid billShippingId);

        Guid Add(Database.Models.BillShippingTotalRule billShippingTotalRule);

        void Update(Database.Models.BillShippingTotalRule billShippingTotalRule);
    }
}