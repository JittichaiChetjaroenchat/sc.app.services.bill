using System;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillShippingRangeRuleRepository : IRepository
    {
        Database.Models.BillShippingRangeRule GetByBillShippingId(Guid billShippingId);

        Guid Add(Database.Models.BillShippingRangeRule billShippingRangeRule);

        void Update(Database.Models.BillShippingRangeRule billShippingRangeRule);
    }
}