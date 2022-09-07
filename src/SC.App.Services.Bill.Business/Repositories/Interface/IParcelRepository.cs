using System;
using System.Collections.Generic;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IParcelRepository : IRepository
    {
        Database.Models.Parcel GetById(Guid id);

        Database.Models.Parcel GetLatestByBillId(Guid billId);

        List<Database.Models.Parcel> GetByIds(Guid[] ids);

        List<Database.Models.Parcel> GetByBillId(Guid billId);

        Guid Add(Database.Models.Parcel parcel);

        void Update(Database.Models.Parcel parcel);

        void Remove(Database.Models.Parcel parcel);
    }
}