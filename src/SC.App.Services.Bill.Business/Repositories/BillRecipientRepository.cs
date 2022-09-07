using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillRecipientRepository : BaseRepository, IBillRecipientRepository
    {
        public BillRecipientRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillRecipient GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillRecipients
                    .Include(x => x.BillRecipientContact)
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public BillRecipient GetByBillId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillRecipients
                    .Include(x => x.BillRecipientContact)
                    .FirstOrDefault(x => x.BillId == billId);
            }
        }

        public Guid Add(BillRecipient billRecipient)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billRecipient);
                context.SaveChanges();

                return billRecipient.Id;
            }
        }

        public void Update(BillRecipient billRecipient)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(billRecipient).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}