using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class PaymentStatusRepository : BaseRepository, IPaymentStatusRepository
    {
        public PaymentStatusRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public PaymentStatus GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.PaymentStatuses
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public PaymentStatus GetByCode(string code)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.PaymentStatuses
                    .FirstOrDefault(x => x.Code == code);
            }
        }
    }
}