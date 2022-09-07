using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillShippingTotalRuleRepository : BaseRepository, IBillShippingTotalRuleRepository
    {
        public BillShippingTotalRuleRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillShippingTotalRule GetByBillShippingId(Guid billShippingId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillShippingTotalRules
                    .FirstOrDefault(x => x.BillShippingId == billShippingId);
            }
        }

        public Guid Add(BillShippingTotalRule billShippingTotalRule)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billShippingTotalRule);
                context.SaveChanges();

                return billShippingTotalRule.Id;
            }
        }

        public void Update(BillShippingTotalRule billShippingTotalRule)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Update(billShippingTotalRule);
                context.SaveChanges();
            }
        }
    }
}