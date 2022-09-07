using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillTagRepository : BaseRepository, IBillTagRepository
    {
        public BillTagRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillTag GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillTags
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public BillTag GetByUniqueKey(Guid billId, Guid tagId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillTags
                    .FirstOrDefault(x => x.BillId == billId && x.TagId == tagId);
            }
        }

        public List<BillTag> GetByBillId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillTags
                    .Where(x => x.BillId == billId)
                    .ToList();
            }
        }

        public Guid Add(BillTag billTag)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billTag);
                context.SaveChanges();

                return billTag.Id;
            }
        }

        public void Remove(BillTag billTag)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Remove(billTag);
                context.SaveChanges();
            }
        }
    }
}