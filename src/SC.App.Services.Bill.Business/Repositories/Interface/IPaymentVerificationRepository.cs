using System;
using SC.App.Services.Bill.Common.Repositories;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IPaymentVerificationRepository : IRepository
    {
        PaymentVerification GetById(Guid id);
    }
}