using System;
using System.Collections.Generic;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IPaymentRepository : IRepository
    {
        Database.Models.Payment GetById(Guid id);

        Database.Models.Payment GetLatestByBilId(Guid billId);

        List<Database.Models.Payment> GetByBilId(Guid billId);

        List<Database.Models.Payment> GetByBilIdAndStatus(Guid billId, string status);

        int Count(Guid billId);

        Guid Add(Database.Models.Payment payment);

        void Update(Database.Models.Payment payment);
    }
}