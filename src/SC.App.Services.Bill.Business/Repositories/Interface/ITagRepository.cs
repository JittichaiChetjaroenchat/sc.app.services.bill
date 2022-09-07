using System;
using System.Collections.Generic;
using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Business.Repositories.Interface
{
    public interface ITagRepository : IRepository
    {
        Database.Models.Tag GetById(Guid id);

        List<Database.Models.Tag> GetByIds(Guid[] ids);

        List<Database.Models.Tag> GetByChannelId(Guid channelId);

        Database.Models.Tag GetByUniqueKey(Guid channelId, string name);

        Guid Add(Database.Models.Tag tag);

        void Update(Database.Models.Tag tag);

        void Remove(Database.Models.Tag tag);

        void Removes(Database.Models.Tag[] tags);
    }
}