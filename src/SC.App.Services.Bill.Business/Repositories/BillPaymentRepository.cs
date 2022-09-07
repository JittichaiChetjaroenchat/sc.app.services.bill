using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillPaymentRepository : BaseRepository, IBillPaymentRepository
    {
        public BillPaymentRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillPayment GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillPayments
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public BillPayment GetByBillId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillPayments
                    .FirstOrDefault(x => x.BillId == billId);
            }
        }

        public Guid Add(BillPayment billPayment)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billPayment);
                context.SaveChanges();

                return billPayment.Id;
            }
        }

        public void Update(BillPayment billPayment)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(billPayment).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}