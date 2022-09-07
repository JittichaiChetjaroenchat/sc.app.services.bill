using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillShippingRangeRuleRepository : BaseRepository, IBillShippingRangeRuleRepository
    {
        public BillShippingRangeRuleRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillShippingRangeRule GetByBillShippingId(Guid billShippingId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillShippingRangeRules
                    .FirstOrDefault(x => x.BillShippingId == billShippingId);
            }
        }

        public Guid Add(BillShippingRangeRule billShippingRangeRule)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billShippingRangeRule);
                context.SaveChanges();

                return billShippingRangeRule.Id;
            }
        }

        public void Update(BillShippingRangeRule billShippingRangeRule)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Update(billShippingRangeRule);
                context.SaveChanges();
            }
        }
    }
}