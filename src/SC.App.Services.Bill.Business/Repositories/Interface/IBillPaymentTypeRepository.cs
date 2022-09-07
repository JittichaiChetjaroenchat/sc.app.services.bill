using System;
using SC.App.Services.Bill.Common.Repositories;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillPaymentTypeRepository : IRepository
    {
        BillPaymentType GetById(Guid id);

        BillPaymentType GetByCode(string code);
    }
}