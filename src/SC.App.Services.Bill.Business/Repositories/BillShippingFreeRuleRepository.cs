using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillShippingFreeRuleRepository : BaseRepository, IBillShippingFreeRuleRepository
    {
        public BillShippingFreeRuleRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillShippingFreeRule GetByBillShippingId(Guid billShippingId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillShippingFreeRules
                    .FirstOrDefault(x => x.BillShippingId == billShippingId);
            }
        }

        public Guid Add(BillShippingFreeRule billShippingFreeRule)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billShippingFreeRule);
                context.SaveChanges();

                return billShippingFreeRule.Id;
            }
        }

        public void Update(BillShippingFreeRule billShippingFreeRule)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Update(billShippingFreeRule);
                context.SaveChanges();
            }
        }
    }
}