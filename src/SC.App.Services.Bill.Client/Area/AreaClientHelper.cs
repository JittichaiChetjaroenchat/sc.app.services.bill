using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Area.Client;

namespace SC.App.Services.Bill.Client.Area
{
    public class AreaClientHelper
    {
        public static bool IsSuccess(ResponseOfGetAreaResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static bool IsSuccess(ResponseOfListOfGetAreaResponse response)
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