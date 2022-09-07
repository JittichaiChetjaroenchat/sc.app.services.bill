using System;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillRecipientContactRepository : IRepository
    {
        Database.Models.BillRecipientContact GetById(Guid id);

        Database.Models.BillRecipientContact GetByBillRecipientId(Guid billRecipientId);

        Guid Add(Database.Models.BillRecipientContact billRecipientContact);

        void Update(Database.Models.BillRecipientContact billRecipientContact);
    }
}