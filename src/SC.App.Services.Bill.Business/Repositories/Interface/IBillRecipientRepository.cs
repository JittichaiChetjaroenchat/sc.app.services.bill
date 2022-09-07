using System;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillRecipientRepository : IRepository
    {
        Database.Models.BillRecipient GetById(Guid id);

        Database.Models.BillRecipient GetByBillId(Guid billId);

        Guid Add(Database.Models.BillRecipient billRecipient);

        void Update(Database.Models.BillRecipient billRecipient);
    }
}