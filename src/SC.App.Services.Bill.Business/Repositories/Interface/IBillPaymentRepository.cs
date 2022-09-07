using System;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillPaymentRepository : IRepository
    {
        Database.Models.BillPayment GetById(Guid id);

        Database.Models.BillPayment GetByBillId(Guid billId);

        Guid Add(Database.Models.BillPayment billPayment);

        void Update(Database.Models.BillPayment billPayment);
    }
}