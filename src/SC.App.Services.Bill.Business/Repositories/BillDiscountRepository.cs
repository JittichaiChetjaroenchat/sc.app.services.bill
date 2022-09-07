using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillDiscountRepository : BaseRepository, IBillDiscountRepository
    {
        public BillDiscountRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillDiscount GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillDiscounts
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public BillDiscount GetByBillId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillDiscounts
                    .FirstOrDefault(x => x.BillId == billId);
            }
        }

        public Guid Add(BillDiscount billDiscount)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billDiscount);
                context.SaveChanges();

                return billDiscount.Id;
            }
        }

        public void Update(BillDiscount billDiscount)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(billDiscount).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}