using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public PaymentRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public Payment GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Payments
                    .Include(x => x.PaymentStatus)
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public Payment GetLatestByBilId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Payments
                    .Include(x => x.PaymentStatus)
                    .OrderByDescending(x => x.PaymentNo)
                    .FirstOrDefault(x => x.BillId == billId);
            }
        }

        public List<Payment> GetByBilId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Payments
                    .Include(x => x.PaymentStatus)
                    .Where(x => x.BillId == billId)
                    .ToList();
            }
        }

        public List<Payment> GetByBilIdAndStatus(Guid billId, string status)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Payments
                    .Include(x => x.PaymentStatus)
                    .Where(x => x.BillId == billId && x.PaymentStatus.Code == status)
                    .ToList();
            }
        }

        public int Count(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Payments
                    .Count(x => x.BillId == billId);
            }
        }

        public Guid Add(Payment payment)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(payment);
                context.SaveChanges();

                return payment.Id;
            }
        }

        public void Update(Payment payment)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(payment).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}