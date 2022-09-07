using System;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillShippingFreeRuleRepository : IRepository
    {
        Database.Models.BillShippingFreeRule GetByBillShippingId(Guid billShippingId);

        Guid Add(Database.Models.BillShippingFreeRule billShippingFreeRule);

        void Update(Database.Models.BillShippingFreeRule billShippingFreeRule);
    }
}