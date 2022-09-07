using System;
using SC.App.Services.Bill.Business.Commands.BillRecipient;
using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Mappers
{
    public class BillRecipientMapper
    {
        public static BillRecipient Map(BillRecipient persistent, UpdateBillRecipient payload)
        {
            if (persistent == null || 
                payload == null)
            {
                return persistent;
            }

            persistent.Name = payload.Name;
            persistent.AliasName = payload.AliasName;
            persistent.UpdatedBy = payload.UserId;
            persistent.UpdatedOn = DateTime.Now;

            return persistent;
        }
    }
}