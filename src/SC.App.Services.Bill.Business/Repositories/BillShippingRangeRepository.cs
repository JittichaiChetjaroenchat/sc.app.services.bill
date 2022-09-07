using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillShippingRangeRepository : BaseRepository, IBillShippingRangeRepository
    {
        public BillShippingRangeRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public List<BillShippingRange> GetByBillShippingRangeRuleId(Guid billShippingRangeRuleId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillShippingRanges
                    .Where(x => x.BillShippingRangeRuleId == billShippingRangeRuleId)
                    .ToList();
            }
        }

        public void Adds(List<BillShippingRange> billShippingRanges)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.AddRange(billShippingRanges);
                context.SaveChanges();
            }
        }

        public void Removes(List<BillShippingRange> billShippingRanges)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.RemoveRange(billShippingRanges);
                context.SaveChanges();
            }
        }
    }
}