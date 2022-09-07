using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BillRecipientContactRepository : BaseRepository, IBillRecipientContactRepository
    {
        public BillRecipientContactRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public BillRecipientContact GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillRecipientContacts
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public BillRecipientContact GetByBillRecipientId(Guid billRecipientId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.BillRecipientContacts
                .FirstOrDefault(x => x.BillRecipientId == billRecipientId);
            }
        }

        public Guid Add(BillRecipientContact billRecipientContact)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(billRecipientContact);
                context.SaveChanges();

                return billRecipientContact.Id;
            }
        }

        public void Update(BillRecipientContact billRecipientContact)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(billRecipientContact).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}