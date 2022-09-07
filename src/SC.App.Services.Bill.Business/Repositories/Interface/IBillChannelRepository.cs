using System;
using SC.App.Services.Bill.Common.Repositories;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillChannelRepository : IRepository
    {
        BillChannel GetById(Guid id);

        BillChannel GetByCode(string code);
    }
}