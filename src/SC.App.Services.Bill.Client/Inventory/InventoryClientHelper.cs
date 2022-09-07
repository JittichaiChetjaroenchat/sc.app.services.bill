using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Inventory.Client;

namespace SC.App.Services.Bill.Client.Inventory
{
    public class InventoryClientHelper
    {
        public static bool IsSuccess(ResponseOfUpdateStockResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static ResponseError GetError(ICollection<ResponseError> errors)
        {
            return errors.FirstOrDefault();
        }

        private static bool IsOk(EnumResponseStatus status)
        {
            return status == EnumResponseStatus.OK;
        }
    }
}