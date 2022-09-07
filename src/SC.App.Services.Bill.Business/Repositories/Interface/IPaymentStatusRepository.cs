using System;
using SC.App.Services.Bill.Common.Repositories;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IPaymentStatusRepository : IRepository
    {
        PaymentStatus GetById(Guid id);

        PaymentStatus GetByCode(string code);
    }
}