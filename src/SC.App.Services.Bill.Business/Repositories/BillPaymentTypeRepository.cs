using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillPaymentTypeRepository : BaseRepository, IBillPaymentTypeRepository
    {
        public BillPaymentTypeRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillPaymentType GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillPaymentTypes
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public BillPaymentType GetByCode(string code)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillPaymentTypes
                    .FirstOrDefault(x => x.Code == code);
            }
        }
    }
}