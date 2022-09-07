using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Business.Repositories.Interface;
using SC.App.Services.Bill.Database;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class ParcelRepository : BaseRepository, IParcelRepository
    {
        public ParcelRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public Parcel GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Parcels
                    .Include(x => x.Bill)
                    .Include(x => x.ParcelStatus)
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public Parcel GetLatestByBillId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Parcels
                    .Include(x => x.Bill)
                    .Include(x => x.ParcelStatus)
                    .OrderByDescending(x => x.CreatedOn)
                    .FirstOrDefault(x => x.BillId == billId);
            }
        }

        public List<Parcel> GetByIds(Guid[] ids)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Parcels
                    .Include(x => x.Bill)
                    .Include(x => x.ParcelStatus)
                    .Where(x => ids.Contains(x.Id))
                    .ToList();
            }
        }

        public List<Parcel> GetByBillId(Guid billId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Parcels
                    .Include(x => x.Bill)
                    .Include(x => x.ParcelStatus)
                    .Where(x => x.BillId == billId)
                    .ToList();
            }
        }

        public Guid Add(Parcel parcel)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(parcel);
                context.SaveChanges();

                return parcel.Id;
            }
        }

        public void Update(Parcel parcel)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(parcel).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Remove(Parcel parcel)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Remove(parcel);
                context.SaveChanges();
            }
        }
    }
}