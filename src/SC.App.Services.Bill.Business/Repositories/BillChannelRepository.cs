using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillChannelRepository : BaseRepository, IBillChannelRepository
    {
        public BillChannelRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public BillChannel GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillChannels
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public BillChannel GetByCode(string code)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillChannels
                    .FirstOrDefault(x => x.Code == code);
            }
        }
    }
}