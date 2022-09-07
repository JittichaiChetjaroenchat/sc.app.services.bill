using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Courier.Client;

namespace SC.App.Services.Bill.Client.Courier
{
    public class CourierClientHelper
    {
        public static bool IsSuccess(ResponseOfGetOrderResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static bool IsSuccess(ResponseOfCreateOrderResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static bool IsSuccess(ResponseOfCancelOrderResponse response)
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