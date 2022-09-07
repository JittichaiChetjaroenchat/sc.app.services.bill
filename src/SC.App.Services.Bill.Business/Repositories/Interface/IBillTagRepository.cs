using System;
using System.Collections.Generic;
using SC.App.Services.Bill.Common.Repositories;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillTagRepository : IRepository
    {
        BillTag GetById(Guid id);

        BillTag GetByUniqueKey(Guid billId, Guid tagId);

        List<BillTag> GetByBillId(Guid billId);

        Guid Add(BillTag billTag);

        void Remove(BillTag billTag);
    }
}