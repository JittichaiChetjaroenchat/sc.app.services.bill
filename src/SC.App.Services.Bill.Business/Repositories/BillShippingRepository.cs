using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillShippingRepository : BaseRepository, IBillShippingRepository
    {
        public BillShippingRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillShipping GetByBillId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillShippings
                    .Include(x => x.BillShippingRangeRule)
                    .ThenInclude(x => x.BillShippingRanges)
                    .Include(x => x.BillShippingTotalRule)
                    .Include(x => x.BillShippingFreeRule)
                    .FirstOrDefault(x => x.BillId == billId);
            }
        }

        public Guid Add(BillShipping billShipping)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billShipping);
                context.SaveChanges();

                return billShipping.Id;
            }
        }

        public void Update(BillShipping billShipping)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(billShipping).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}