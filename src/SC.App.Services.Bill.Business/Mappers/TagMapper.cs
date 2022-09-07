using System;
using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Commands.Tag;
using SC.App.Services.Bill.Business.Queries.Tag;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class TagMapper
    {
        public static List<GetTagResponse> Map(List<Tag> tags)
        {
            return tags
                .Select(Map)
                .ToList();
        }

        public static GetTagResponse Map(Tag tag)
        {
            if (tag == null)
            {
                return null;
            }

            return new GetTagResponse
            {
                Id = tag.Id,
                ChannelId = tag.ChannelId,
                Color = tag.Color,
                Name = tag.Name,
                Description = tag.Description
            };
        }

        public static Tag Create(Guid id, CreateTag tag)
        {
            return new Tag
            {
                Id = id,
                ChannelId = tag.ChannelId,
                Color = tag.Color,
                Name = tag.Name,
                Description = tag.Description,
                CreatedBy = tag.UserId,
                UpdatedBy = tag.UserId,
            };
        }

        public static void Update(UpdateTag src, Tag dest)
        {
            dest.Color = src.Color;
            dest.Name = src.Name;
            dest.Description = src.Description;
            dest.UpdatedBy = src.UserId;
            dest.UpdatedOn = DateTime.Now;
        }
    }
}