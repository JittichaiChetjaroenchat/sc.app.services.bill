using System;
using SC.App.Services.Bill.Business.Commands.BillRecipient;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class BillRecipientContactMapper
    {
        public static BillRecipientContact Map(Guid billRecipientId, UpdateBillRecipientContact payload, Guid userId)
        {
            if (payload == null)
            {
                return null;
            }

            return new BillRecipientContact
            {
                BillRecipientId = billRecipientId,
                Address = payload.Address,
                AreaId = payload.AreaId,
                PrimaryPhone = payload.PrimaryPhone,
                SecondaryPhone = payload.SecondaryPhone,
                Email = payload.Email,
                CreatedBy = userId,
                UpdatedBy = userId
            };
        }

        public static BillRecipientContact Map(BillRecipientContact persistent, UpdateBillRecipientContact payload, Guid userId)
        {
            if (persistent == null ||
                payload == null)
            {
                return persistent;
            }

            persistent.Address = payload.Address;
            persistent.AreaId = payload.AreaId;
            persistent.PrimaryPhone = payload.PrimaryPhone;
            persistent.SecondaryPhone = payload.SecondaryPhone;
            persistent.Email = payload.Email;
            persistent.UpdatedBy = userId;
            persistent.UpdatedOn = DateTime.Now;

            return persistent;
        }
    }
}