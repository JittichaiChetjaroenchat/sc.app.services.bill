using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillConfigurationRepository : BaseRepository, IBillConfigurationRepository
    {
        public BillConfigurationRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public BillConfiguration GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillConfigurations
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public BillConfiguration GetByChannelId(Guid channelId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillConfigurations
                    .FirstOrDefault(x => x.ChannelId == channelId);
            }
        }

        public Guid Add(BillConfiguration billConfiguration)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billConfiguration);
                context.SaveChanges();

                return billConfiguration.Id;
            }
        }

        public void Update(BillConfiguration billConfiguration)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(billConfiguration).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}