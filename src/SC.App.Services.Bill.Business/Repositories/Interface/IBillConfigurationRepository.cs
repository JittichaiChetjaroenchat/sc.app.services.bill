using System;
using SC.App.Services.Bill.Common.Repositories;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface IBillConfigurationRepository : IRepository
    {
        BillConfiguration GetById(Guid id);

        BillConfiguration GetByChannelId(Guid channelId);

        Guid Add(BillConfiguration billConfiguration);

        void Update(BillConfiguration billConfiguration);
    }
}