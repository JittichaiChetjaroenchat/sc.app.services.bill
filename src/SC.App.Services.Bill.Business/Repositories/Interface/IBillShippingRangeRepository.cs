using System;
using System.Collections.Generic;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillShippingRangeRepository : IRepository
    {
        List<Database.Models.BillShippingRange> GetByBillShippingRangeRuleId(Guid billShippingRangeRuleId);

        void Adds(List<Database.Models.BillShippingRange> billShippingRanges);

        void Removes(List<Database.Models.BillShippingRange> billShippingRanges);
    }
}