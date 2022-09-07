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
    public class TagRepository : BaseRepository, ITagRepository
    {
        public TagRepository(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        public Tag GetById(Guid id)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Tags
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public List<Tag> GetByIds(Guid[] ids)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Tags
                    .Where(x => ids.Contains(x.Id))
                    .OrderBy(o => o.CreatedOn)
                    .ToList();
            }
        }

        public List<Tag> GetByChannelId(Guid channelId)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Tags
                    .Where(x => x.ChannelId == channelId)
                    .OrderBy(o => o.CreatedOn)
                    .ToList();
            }
        }

        public Tag GetByUniqueKey(Guid channelId, string name)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                return context.Tags
                    .FirstOrDefault(x => x.ChannelId == channelId && x.Name == name);
            }
        }

        public Guid Add(Tag tag)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Add(tag);
                context.SaveChanges();

                return tag.Id;
            }
        }

        public void Update(Tag tag)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Entry(tag).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Remove(Tag tag)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.Remove(tag);
                context.SaveChanges();
            }
        }

        public void Removes(Tag[] tags)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                context.RemoveRange(tags);
                context.SaveChanges();
            }
        }
    }
}