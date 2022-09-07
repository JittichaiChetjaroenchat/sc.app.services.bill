using System;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillShippingRepository : IRepository
    {
        Database.Models.BillShipping GetByBillId(Guid billId);

        Guid Add(Database.Models.BillShipping billShipping);

        void Update(Database.Models.BillShipping billShipping);
    }
}