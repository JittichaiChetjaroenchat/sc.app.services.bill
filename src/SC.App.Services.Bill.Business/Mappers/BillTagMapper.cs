using System;
using SC.App.Services.Bill.Business.Commands.BillTag;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class BillTagMapper
    {
        public static BillTag Create(Guid id, CreateBillTag data)
        {
            return new BillTag
            {
                Id = id,
                BillId = data.BillId,
                TagId = data.TagId,
                CreatedBy = data.UserId,
                UpdatedBy = data.UserId
            };
        }

        public static BillTag Update(Guid id, Guid tagId, UpdateBillTag data)
        {
            return new BillTag
            {
                Id = id,
                BillId = data.BillId,
                TagId = tagId,
                CreatedBy = data.UserId,
                UpdatedBy = data.UserId
            };
        }
    }
}