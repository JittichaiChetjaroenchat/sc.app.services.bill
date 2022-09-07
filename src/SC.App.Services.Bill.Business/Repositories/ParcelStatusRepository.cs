using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class ParcelStatusRepository : BaseRepository, IParcelStatusRepository
    {
        public ParcelStatusRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public ParcelStatus GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.ParcelStatuses
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public ParcelStatus GetByCode(string code)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.ParcelStatuses
                    .FirstOrDefault(x => x.Code == code);
            }
        }
    }
}