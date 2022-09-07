using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class PaymentVerificationRepository : BaseRepository, IPaymentVerificationRepository
    {
        public PaymentVerificationRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public PaymentVerification GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.PaymentVerifications
                    .Include(x => x.Payment)
                    .ThenInclude(x => x.Bill)
                    .Include(x => x.PaymentVerificationDetail)
                    .Include(x => x.PaymentVerificationStatus)
                    .FirstOrDefault(x => x.Id == id);
            }
        }
    }
}