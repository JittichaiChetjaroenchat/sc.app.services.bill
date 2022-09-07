using System;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillDiscountRepository : IRepository
    {
        Database.Models.BillDiscount GetById(Guid id);

        Database.Models.BillDiscount GetByBillId(Guid billId);

        Guid Add(Database.Models.BillDiscount billDiscount);

        void Update(Database.Models.BillDiscount billDiscount);
    }
}